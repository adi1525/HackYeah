import numpy as np
import cv2
import imutils
import socket as sc
from socket import *

#cap = cv2.VideoCapture(0)

UDP_IP = "255.255.255.255"
UDP_PORT = 2069

cap = cv2.VideoCapture('rtsp://hackathon:!Hackath0n@192.168.0.2')
cap.set(cv2.CAP_PROP_BUFFERSIZE,0)

def GetStartingRect():
    while(True):

        ret, frame = cap.read()
        frame = cv2.resize(frame,(320,180))
        frame = cv2.medianBlur(frame, 5)
        grayFrame = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
        circles	= cv2.HoughCircles(grayFrame,cv2.HOUGH_GRADIENT,1,300,param1=50,param2=30,minRadius=60,maxRadius=72)
        if circles is None:
            continue
        x1 = int(circles[0][0][0] - circles[0][0][2] - 2)
        x2 = int(circles[0][0][0] + circles[0][0][2] + 2)
        y1 = int(circles[0][0][1] - circles[0][0][2] - 2)
        y2 = int(circles[0][0][1] + circles[0][0][2] + 2)
        return (x1,x2,y1,y2)

rect = GetStartingRect()

def GetThemReds(src):
    src[:,:,2] = src[:,:,2] - src[:,:,1]
    src[:,:,0] = src[:,:,2]
    src[:,:,1] = src[:,:,2]

    return src




while(True):
    
# Capture frame-by-frame
    ret, frame = cap.read()
    frame = cv2.resize(frame,(320,180))
    frame = frame[rect[2]:rect[3],rect[0]:rect[1]]
    

    

    redFrame = GetThemReds(frame)

    ret,inv = cv2.threshold(redFrame,200,255,cv2.THRESH_TOZERO_INV)
    ret,inv = cv2.threshold(inv,60,255,cv2.THRESH_BINARY)

    grayFrame = cv2.cvtColor(inv, cv2.COLOR_BGR2GRAY)
    conts,h=cv2.findContours(grayFrame.copy(),cv2.RETR_EXTERNAL,cv2.CHAIN_APPROX_NONE)
    drawTarget = inv
    cv2.drawContours(drawTarget,conts,-1,(255,0,0),1)
    cv2.circle(drawTarget,(int(drawTarget.shape[0]/2),int(drawTarget.shape[1]/2)),2,(0,255,0))
    cv2.circle(drawTarget,(int(drawTarget.shape[0]/2),int(0)),2,(0,255,0))
    
    if conts is not None:
        x,y,w,h=cv2.boundingRect(conts[0])

        cX = int(x +w/2)
        cY = int(y +h/2)

        cv2.line(drawTarget, (cX, cY), (int(inv.shape[0]/2), int(inv.shape[1]/2)), (0,255,0), 1)
        cv2.line(drawTarget, (int(inv.shape[0]/2), 0), (int(inv.shape[0]/2), int(inv.shape[1]/2)), (0,255,0), 1)
        #thresh2[:,:,:] = thresh2[:,:,:] * frame[:,:,:] / 255
        cv2.imshow('frame', drawTarget)

        x = np.array([cY-int(drawTarget.shape[1]/2)])
        y = np.array([cX-int(drawTarget.shape[0]/2)])
        tan = np.arctan2(x,y) *180 / np.pi

        sock = sc.socket(sc.AF_INET,sc.SOCK_DGRAM)
        sock.setsockopt(SOL_SOCKET, SO_REUSEADDR,1)
        sock.setsockopt(SOL_SOCKET, SO_BROADCAST,1)
        sock.sendto(tan, (UDP_IP, UDP_PORT))
        #print(tan[0] * 180 / 3.14)
        if cv2.waitKey(1) & 0xFF == ord('q'):
            break

# When everything done, release the capture
cap.release()
cv2.destroyAllWindows()