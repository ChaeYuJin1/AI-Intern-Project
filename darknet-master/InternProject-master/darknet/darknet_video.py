from ctypes import *
import math
import random
import os
import cv2
import numpy as np
import time
import scipy.io as sio
import scipy.io.wavfile
import sounddevice as sd
import socket

from queue import Queue

import darknet
import threading
from _thread import *


ID_STRING = '1;CAM'
DANGER_PACKET = '1danger'
SAFETY_PACKET = '1safety'
CHECK_PACKET = '1check'
HOST = '141.223.107.158'
PORT = 8888

enclosure_queue_img = Queue()
enclosure_queue_state = Queue()


def convertBack(x, y, w, h):
    xmin = int(round(x - (w / 2)))
    xmax = int(round(x + (w / 2)))
    ymin = int(round(y - (h / 2)))
    ymax = int(round(y + (h / 2)))
    return xmin, ymin, xmax, ymax


def cvDrawBoxes(detections, img):
    for detection in detections:
        x, y, w, h = detection[2][0],\
            detection[2][1],\
            detection[2][2],\
            detection[2][3]
        xmin, ymin, xmax, ymax = convertBack(
            float(x), float(y), float(w), float(h))
        pt1 = (xmin, ymin)
        pt2 = (xmax, ymax)
        cv2.rectangle(img, pt1, pt2, (0, 255, 0), 1)
        cv2.putText(img,
                    detection[0].decode() +
                    " [" + str(round(detection[1] * 100, 2)) + "]",
                    (pt1[0], pt1[1] - 5), cv2.FONT_HERSHEY_SIMPLEX, 0.5,
                    [0, 255, 0], 2)
    return img


netMain = None
metaMain = None
altNames = None

# read audio file
samplerate, data = sio.wavfile.read('announcement.wav')

def playSound():
    sd.play(data)

def YOLO(queue_img, queue_state):

    global metaMain, netMain, altNames

    # Yolo v2 tiny
    # configPath = "./cfg/yolov2-tiny_obj.cfg"
    # weightPath = "./backup/yolov2-tiny_obj_best_256_iter8000.weights"

    # Yolo v3 tiny
    # configPath = "./cfg/yolov3-tiny_obj.cfg"
    # weightPath = "./backup/yolov3-tiny_obj_best_416.weights"

    # Yolo v3
    configPath = "./cfg/yolov3_416.cfg"
    weightPath = "./backup/yolov3_416_best.weights"

    # Yolo v3 5 layer
    # configPath = "./cfg/yolov3_5l.cfg"
    # weightPath = "./backup/yolov3_5l_best.weights"

    metaPath = "./data/obj.data"
    if not os.path.exists(configPath):
        raise ValueError("Invalid config path `" +
                         os.path.abspath(configPath)+"`")
    if not os.path.exists(weightPath):
        raise ValueError("Invalid weight path `" +
                         os.path.abspath(weightPath)+"`")
    if not os.path.exists(metaPath):
        raise ValueError("Invalid data file path `" +
                         os.path.abspath(metaPath)+"`")
    if netMain is None:
        netMain = darknet.load_net_custom(configPath.encode(
            "ascii"), weightPath.encode("ascii"), 0, 1)  # batch size = 1
    if metaMain is None:
        metaMain = darknet.load_meta(metaPath.encode("ascii"))
    if altNames is None:
        try:
            with open(metaPath) as metaFH:
                metaContents = metaFH.read()
                import re
                match = re.search("names *= *(.*)$", metaContents,
                                  re.IGNORECASE | re.MULTILINE)
                if match:
                    result = match.group(1)
                else:
                    result = None
                try:
                    if os.path.exists(result):
                        with open(result) as namesFH:
                            namesList = namesFH.read().strip().split("\n")
                            altNames = [x.strip() for x in namesList]
                except TypeError:
                    pass
        except Exception:
            pass
    cap = cv2.VideoCapture(0)
    # cap = cv2.VideoCapture("./data/img/IMG_3825.MOV")
    cap.set(3, 416)
    cap.set(4, 416)
    # out = cv2.VideoWriter(
    #     "output.avi", cv2.VideoWriter_fourcc(*"MJPG"), 60.0,
    #     (darknet.network_width(netMain), darknet.network_height(netMain)))
    print("Starting the YOLO loop...")

    # Create an image we reuse for each detect
    darknet_image = darknet.make_image(darknet.network_width(netMain),
                                    darknet.network_height(netMain), 3)



    # Initialize init_fps
    init_fps = -1
    prev_alarm = -1
    cur_alarm = -1
    cnt = 0
    danger = 0
    is_check_mode = False
    safety_changed_time = time.time()
    prev_sent_time = time.time()

    current_state = SAFETY_PACKET
    while True:
        prev_time = time.time()
        ret, frame_read = cap.read()
        frame_rgb = cv2.cvtColor(frame_read, cv2.COLOR_BGR2RGB)
        frame_resized = cv2.resize(frame_rgb,
                                   (darknet.network_width(netMain),
                                    darknet.network_height(netMain)),
                                   interpolation=cv2.INTER_LINEAR)

        darknet.copy_image_from_bytes(darknet_image,frame_resized.tobytes())

        detections = darknet.detect_image(netMain, metaMain, darknet_image, thresh=0.3)
        image = cvDrawBoxes(detections, frame_resized)
        image = cv2.cvtColor(image, cv2.COLOR_BGR2RGB)

        # memorize fps and if it is first fps, memorize initial fps too.
        fps = 1/(time.time()-prev_time)
        if init_fps == -1 and fps > 1:
            init_fps = int(fps)

        # print(detections)
        # print(fps)


        encode_param = [int(cv2.IMWRITE_JPEG_QUALITY), 90]
        result, imgencode = cv2.imencode('.jpg', image, encode_param)

        data = np.array(imgencode)
        stringData = data.tostring()

        # while not queue_img.empty(): continue
        # queue_img.put(stringData)
        if not queue_img.empty():
            queue_img.queue.clear()

        queue_img.put(stringData)

        # cv2.imshow('Demo', image)

        # judge danger or not
        cnt += 1
        for i in detections:
            if i[0].decode("utf-8") == "warning":
                danger += 1
                break



        if cnt == init_fps*2:
            if danger > cnt * 0.8:
                current_state = DANGER_PACKET
                cur_alarm = time.time()
                if prev_alarm == -1 or cur_alarm - prev_alarm > 8:
                    prev_alarm = time.time()
                    print("it is really danger")
                    playSound()
                is_check_mode = False
            else:
                if current_state == DANGER_PACKET:
                    safety_changed_time = time.time()
                    print(safety_changed_time)
                    current_state = SAFETY_PACKET
                    is_check_mode = False
                elif current_state == CHECK_PACKET:
                    current_state = CHECK_PACKET
                    is_check_mode = True

                else:
                    if time.time() - safety_changed_time > 20:
                        current_state = CHECK_PACKET
                        is_check_mode = True
                    else:
                        current_state = SAFETY_PACKET
                        is_check_mode = False

            cnt = 0
            danger = 0

        if is_check_mode:
            if time.time() - prev_sent_time > 20:
                if not queue_state.empty():
                    queue_state.queue.clear()
                queue_state.put(current_state)
                prev_sent_time = time.time()
        else:
            if not queue_state.empty():
                queue_state.queue.clear()
            queue_state.put(current_state)

        if cv2.waitKey(1) == ord('q'):
            break

    cap.release()
    # out.release()



def communication(queue_img, queue_state):

    client_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    print("try connect")
    client_socket.connect((HOST, PORT))

    print("connected")
    client_socket.send(ID_STRING.encode())

    while True:
        data = client_socket.recv(2)

        if data == b'O' or data == b'B':
            while queue_state.empty(): continue
            stateString = queue_state.get()
            client_socket.send(stateString.encode())
        elif data == b'A':
            while queue_img.empty(): continue
            stringData = queue_img.get()
            client_socket.send(stringData)
        else:
            continue


if __name__ == "__main__":
    start_new_thread(communication, (enclosure_queue_img, enclosure_queue_state))
    YOLO(enclosure_queue_img, enclosure_queue_state)