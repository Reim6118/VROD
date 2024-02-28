# VROD
#### This research aims to create an intuitive way to control AGV for use in scenario involving varying levels of luminosity in the environment.

A VR application is developed with Unity to control the ROS framework AGV, without the use of VR controller, one can directly control the AGV with their head tilting to different angles. Meanwhile, the visual image seen from the AGV is stream into the VR with the slam mapping showing on the top left corner of VR.
- Head up -> robot move forward
- Down -> backwards
- Looking left -> turn left
- Looking right -> turn right

## System Framework
<img width="500" alt="system_overview" src="https://github.com/Reim6118/VROD/assets/32570797/7d939e94-cfe3-403a-aff6-c2940f848c0f">

#### The system can be divided into two sections:&nbsp; 1.VR application built with Unity &nbsp;  2.Nodes set up in Ros environment 

## VR Application

### Control AGV


1. To control the AGV intuitively, gyroscope data is first extracted from VR. 
2. The quaternion data is converted to Euler angles before being calculated into linear and angular velocity for each axis (X, Y, Z).
3. The velocity data is then converted into Twist data that can be recognized by the ROS system.
4. The Twist data is later published onto the cmd_vel topic to directly control the AGV with the help of the RosBridge library.

### Receive visual data from AGV

1. Receive the video stream from the Realsense camera and Slam visualization data through a dedicated server running on the Linux system of the AGV.
2. Receive thermal video streaming that is passed through obejct detection model built with Yolov3.
<img width="300" alt="system_overview1" src="https://github.com/Reim6118/VROD/assets/32570797/88c7983a-95ec-47fd-93d0-3dad8094deb6">

## ROS Environment

### Send visual data to VR
Laser scan data is transmitted to the slam_gmapping node for processing, and the resulting SLAM data is visualized using the rviz package. Subsequently, the visualized SLAM data, along with the video feed from the RealSense camera, is streamed from a server established on the Linux system of the AGV.

### Receive movement command from AGV
Subscribe to the cmd_vel topic to retrieve Twist data, which has been previously converted from the gyro data acquired from the VR sensors.    


<!--## Thermal Image Object Detection



<img width="300" alt="system_overview2" src="https://github.com/Reim6118/VROD/assets/32570797/f7b86465-9064-44b1-8d12-7f53589df2a9"> -->
