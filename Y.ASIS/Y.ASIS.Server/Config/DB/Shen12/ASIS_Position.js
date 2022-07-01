/*
 Navicat Premium Data Transfer

 Source Server         : localhost
 Source Server Type    : MongoDB
 Source Server Version : 40400
 Source Host           : localhost:27017
 Source Schema         : ASIS

 Target Server Type    : MongoDB
 Target Server Version : 40400
 File Encoding         : 65001

 Date: 30/05/2022 14:43:55
*/


// ----------------------------
// Collection structure for ASIS_Position
// ----------------------------
db.getCollection("ASIS_Position").drop();
db.createCollection("ASIS_Position");

// ----------------------------
// Documents of ASIS_Position
// ----------------------------

db.getCollection("ASIS_Position").insert([ {
    ID: 45,
    Index: 45,
    Name: "A",
    OpcConfig: {
        Address: "opc.tcp://10.60.25.2:4840",
        Username: "yd0484",
        Password: "yd123456"
    },
    VideoIds: [
  
    ],
	ExtraVideoIds: [
     
    ],
	DeviceIds: [
     
    ]
} ]);


db.getCollection("ASIS_Position").insert([ {
    ID: 46,
    Index: 46,
    Name: "A",
    OpcConfig: {
        Address: "opc.tcp://10.60.25.2:4840",
        Username: "yd0484",
        Password: "yd123456"
    },
    VideoIds: [

    ],
	ExtraVideoIds: [
           
    ],
	DeviceIds: [

    ]
} ]);


db.getCollection("ASIS_Position").insert([ {
    ID: 47,
    Index: 47,
    Name: "A",
    OpcConfig: {
        Address: "opc.tcp://10.60.26.2:4840",
        Username: "yd0484",
        Password: "yd123456"
    },
    VideoIds: [     

    ],
	ExtraVideoIds: [

    ],
	DeviceIds: [

    ]
}]);

db.getCollection("ASIS_Position").insert([{
    ID: 48,
    Index: 48,
    Name: "A",
    OpcConfig: {
        Address: "opc.tcp://10.60.27.2:4840",
        Username: "yd0484",
        Password: "yd123456"
    },
    VideoIds: [

    ],
    ExtraVideoIds: [

    ],
    DeviceIds: [

    ]
}]);
