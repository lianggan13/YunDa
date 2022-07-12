/*
 Navicat Premium Data Transfer

 Source Server         : 192.168.1.102
 Source Server Type    : MongoDB
 Source Server Version : 40400
 Source Host           : 192.168.1.102:27017
 Source Schema         : ASIS

 Target Server Type    : MongoDB
 Target Server Version : 40400
 File Encoding         : 65001

 Date: 12/07/2022 13:35:43
*/


// ----------------------------
// Collection structure for ASIS_VideoStream
// ----------------------------
db.getCollection("ASIS_VideoStream").drop();
db.createCollection("ASIS_VideoStream");

// ----------------------------
// Documents of ASIS_VideoStream
// ----------------------------
db.getCollection("ASIS_VideoStream").insert([{
    _id: ObjectId("6094f82051430000090077eb"),
    ID: 1,
    Name: "1#防区门",
    Type: "Platform",
    Url: "rtsp://admin:yunda123@192.168.1.195:554/h264/ch1/sub/av_stream",
    Channel: 33,
    Ip: "192.168.1.195",
    Model: "HIK",
    Extension: ""
}]);
db.getCollection("ASIS_VideoStream").insert([{
    _id: ObjectId("60dffce5c44c0000e0004f50"),
    ID: 2,
    Name: "1#股道1列位车号识别",
    Type: "TrainNo",
    Url: "rtsp://admin:yunda123@192.168.1.194:554/h264/ch1/sub/av_stream",
    Channel: 34,
    Ip: "192.168.1.194",
    Model: "HIK",
    Extension: ""
}]);
db.getCollection("ASIS_VideoStream").insert([{
    _id: ObjectId("60e12ac2c44c0000e0004f51"),
    ID: 3,
    Name: "1#股道1列位上侧光栅",
    Type: "TrainNo",
    Url: "rtsp://admin:yunda123@192.168.1.193:554/h264/ch1/sub/av_stream",
    Channel: 35,
    Ip: "192.168.1.193",
    Model: "HIK",
    Extension: ""
}]);
db.getCollection("ASIS_VideoStream").insert([{
    _id: ObjectId("60e12b8ec44c0000e0004f53"),
    ID: 4,
    Name: "1#股道1列位隔离开关",
    Type: "Isolation",
    Url: "rtsp://admin:yunda123@192.168.1.196:554/h264/ch1/sub/av_stream",
    Channel: 37,
    Ip: "192.168.1.196",
    Model: "HIK",
    Extension: ""
}]);
db.getCollection("ASIS_VideoStream").insert([{
    _id: ObjectId("6284a64ecc58000026005a55"),
    ID: 6,
    Name: "1#股道隔离开关",
    Type: "Isolation",
    Url: "rtsp://admin:yunda123@192.168.1.109:554/cam/realmonitor?channel=1&subtype=1",
    Channel: 0,
    Ip: "192.168.1.109",
    Port: 37777,
    UserName: "admin",
    Password: "yunda123",
    Model: "DaHua",
    Extension: ""
}]);
db.getCollection("ASIS_VideoStream").insert([{
    _id: ObjectId("6284a64ecc58000026005a56"),
    ID: 7,
    Name: "1#股道隔离开关",
    Type: "Isolation",
    Url: "rtsp://admin:yunda123@192.168.1.109:554/cam/realmonitor?channel=1&subtype=1",
    Channel: 0,
    Ip: "192.168.1.109",
    Port: 37777,
    UserName: "admin",
    Password: "yunda123",
    Model: "DaHua",
    Extension: ""
}]);
db.getCollection("ASIS_VideoStream").insert([{
    _id: ObjectId("6284a64ecc58000026005a57"),
    ID: 8,
    Name: "1#股道隔离开关",
    Type: "Isolation",
    Url: "rtsp://admin:yunda123@192.168.1.109:554/cam/realmonitor?channel=1&subtype=1",
    Channel: 0,
    Ip: "192.168.1.109",
    Port: 37777,
    UserName: "admin",
    Password: "yunda123",
    Model: "DaHua",
    Extension: ""
}]);
db.getCollection("ASIS_VideoStream").insert([{
    _id: ObjectId("6284a64ecc58000026005a58"),
    ID: 9,
    Name: "1#股道隔离开关",
    Type: "Isolation",
    Url: "rtsp://admin:yunda123@192.168.1.109:554/cam/realmonitor?channel=1&subtype=1",
    Channel: 0,
    Ip: "192.168.1.109",
    Port: 37777,
    UserName: "admin",
    Password: "yunda123",
    Model: "DaHua",
    Extension: ""
}]);
db.getCollection("ASIS_VideoStream").insert([{
    _id: ObjectId("6284a64ecc58000026005a59"),
    ID: 10,
    Name: "1#股道隔离开关",
    Type: "Isolation",
    Url: "rtsp://admin:yunda123@192.168.1.109:554/cam/realmonitor?channel=1&subtype=1",
    Channel: 0,
    Ip: "192.168.1.109",
    Port: 37777,
    UserName: "admin",
    Password: "yunda123",
    Model: "DaHua",
    Extension: ""
}]);
db.getCollection("ASIS_VideoStream").insert([{
    _id: ObjectId("6284a64ecc58000026005a5a"),
    ID: 11,
    Name: "1#股道隔离开关",
    Type: "Isolation",
    Url: "rtsp://admin:yunda123@192.168.1.109:554/cam/realmonitor?channel=1&subtype=1",
    Channel: 0,
    Ip: "192.168.1.109",
    Port: 37777,
    UserName: "admin",
    Password: "yunda123",
    Model: "DaHua",
    Extension: ""
}]);
