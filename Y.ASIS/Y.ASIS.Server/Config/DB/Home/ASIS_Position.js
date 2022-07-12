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
    ID: 1,
    Index: 1,
    Name: "1#股道I列位",
    OpcConfig: {
        Address: "opc.tcp://192.168.2.30:4840",
        Username: "yd0484",
        Password: "yd123456"
    },
    VideoIds: [
        1,
        2,
        3,
        4
    ],
	ExtraVideoIds: [
    
    ],
	DeviceIds: [
		2,
    ]
} ]);

db.getCollection("ASIS_Position").insert([ {
    ID: 2,
    Index: 2,
    Name: "2#股道I列位",
    OpcConfig: {
        Address: "opc.tcp://192.168.2.30:4840",
        Username: "yd0484",
        Password: "yd123456"
    },
    VideoIds: [
        1,
        2,
        3,
        4
    ],
	ExtraVideoIds: [
           
    ],
	DeviceIds: [
        2,
    ]
} ]);

db.getCollection("ASIS_Position").insert([ {
    ID: 3,
    Index: 3,
    Name: "3#股道I列位",
    OpcConfig: {
        Address: "opc.tcp://192.168.2.30:4840",
        Username: "yd0484",
        Password: "yd123456"
    },
    VideoIds: [
        1,
        2,
        3,
        4
    ],
    ExtraVideoIds: [

    ],
    DeviceIds: [
        2,
    ]
} ]);


db.getCollection("ASIS_Position").insert([ {
    ID: 4,
    Index: 4,
    Name: "3#股道II列位",
    OpcConfig: {
        Address: "opc.tcp://192.168.2.30:4840",
        Username: "yd0484",
        Password: "yd123456"
    },
    VideoIds: [
        1,
        2,
        3,
        4
    ],
    ExtraVideoIds: [

    ],
    DeviceIds: [
        2,
    ]
} ]);