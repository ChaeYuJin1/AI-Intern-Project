AlexeyAB/darknet custom object 학습하는 법 (yolov3-tiny.ver)

1. 우선 cfg/yolov3-tiny_obj.cfg 파일을 수정해줍니다. test 부분이 주석처리 되어있지 않다면 우선 test 부분을 주석처리 해줍니다.

1-1.

첫번째로 train 부분을 수정합니다.
batch = 64, subdivision = 16으로 변경합니다.
max_batches는 class의 갯수 * 2000으로 변경합니다. 저의 경우에는 class의 갯수가 2개였기 때문에, 4000으로 변경하였습니다.
steps는 max_batches의 80%와 90%로 변경합니다. 저는 3200, 3600으로 변경하였습니다.
network의 사이즈는 width = 416, height = 416이 default이나, 32의 배수는 모두 가능합니다.

1-2.

다음으로 yolo layer를 변경해줍니다.
classes를 본인이 원하는 class의 갯수로 변경합니다. 저는 2로 변경했습니다.


1-3.

yolo layer 바로 위의 convolutional layer에서 filters를 변경해줍니다.
초기값은 filters = 255이지만, filters = (classes+5)*3 으로 변경합니다.
정확한 공식은 (classes+1+coords)*mask입니다. 만약 v2 버전을 사용하시게 되면 mask가 없는데 이때는 mask 대신 num을 사용해주시면 됩니다.
class가 2이므로 21로 바꿔주었습니다.


2. obj.names 파일을 생성해줍니다.

directory는 항상 darknet.exe를 기준으로 생각하면 됩니다.
저는 이미지를 data/obj 폴더 안에 넣을 것이므로, data/ 안에 obj.names를 만들었습니다.
내용은 다음과 같습니다.

safe
warning

annotation tool을 사용해서 class에 해당하는 번호를 부여했을 때, 해당 클래스의 이름을 지정해주는 파일입니다.


3. obj.data 파일을 생성해줍니다.

해당 파일 또한 data/ 안에 obj.data를 만들었습니다.
내용은 다음과 같습니다.

classes= 2
train  = data/train.txt
valid  = data/test.txt
names = data/obj.names
backup = backup/

이 때, 저는 validation set을 사용해서 mAP를 계산할 예정이므로, 따로 validation의 경로를 추가하였으나, 필요하지 않다면 valid 부분은 사용하지 않아도 무방합니다.


4. train에 사용할 이미지 파일을 넣어줍니다.

저의 경우에는 data/obj 안에 넣어주었고, 이미지와 annotation tool을 통해 생성한 txt 파일이 함께 들어가야 합니다.

4-1 이미지를 labeling 하는 방법은 다음과 같습니다.

https://github.com/AlexeyAB/Yolo_mark

해당 주소를 clone합니다.
이 후, 원하는 경로에 이미지를 넣고 실행 파일 내의 이미지 경로를 변경해줍니다.
저의 경우 ubuntu를 사용하였기 때문에 linux_mark.sh 파일에서 이미지의 경로를 변경해주었고, 이미지는 data/obj 에 넣을 것이었으므로 Yolo_mark 폴더 내부에 data/obj 폴더를 생성하여 그 안에 넣어주었습니다.
이후 bash linux_mark.sh 명령어를 터미널 창에서 실행하면 자동으로 train.txt 파일이 생성됩니다. 해당 파일을 darknet 폴더 안의 data/ 안에 넣어줍니다.


5. pretrained 모델을 다운로드 받아줍니다.

저의 경우에는 yolov3-tiny.conv.11 파일을 다운로드 받았습니다.
경로는 다음과 같습니다.

https://drive.google.com/file/d/18v36esoXCh-PsOKwyP2GWrpYDptDY8Zf/view?usp=sharing


6. training을 시작합니다.

우분투에서의 명령어는 다음과 같습니다.

./darknet detector train data/obj.data cfg/yolov3-tiny_obj.cfg yolov3-tiny.conv.11

이 때, mAP를 계산하고 싶다면 명령어는 다음과 같습니다.

./darknet detector train data/obj.data cfg/yolov3-tiny_obj.cfg yolov3-tiny.conv.11 -map

darknet 실행 파일의 경로에 backup/ 폴더가 있어야 weight가 저장됩니다.
weight는 100 iteration 마다 yolov3-tiny_obj_last.weights 파일 저장이 되고, 1000 iteration이 되면 yolov3-tiny_obj_X000.weights 파일이 저장되게 됩니다.


7. 이 후 해당 weight로 test를 하는 경우, 명령어는 다음과 같습니다.

./darknet detector test data/obj.data cfg/yolov3-tiny_obj.cfg yolov3-tiny_obj_4000.weights

또는 영상을 test 하시고 싶은 경우, 다음 명령어로 가능합니다.

./darknet detector demo data/obj.data cfg/yolov3-tiny_obj.cfg yolov3-tiny_obj_4000.weights -ext_output test.mp4

웹캠 버전!

./darknet detector demo data/obj.data cfg/yolov3-tiny_obj.cfg yolov3-tiny_obj_4000.weights -c 0

