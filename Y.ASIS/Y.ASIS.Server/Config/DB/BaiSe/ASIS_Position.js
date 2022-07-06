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

// 6#道I列位
db.getCollection("ASIS_Position").insert([ {
    ID: 1,
    Index: 1,
    Name: "6#道I列位",
    OpcConfig: {
        // Address: "opc.tcp://192.168.2.30:4840",
        Address: "opc.tcp://10.6.1.20:4840",
        Username: "yd0484",
        Password: "yd123456"
    },
    VideoIds: [
        2,
        3,
        9,
		10,
        11,
        12
    ],
	ExtraVideoIds: [
        1,
        4,
        5,
		6,
        7,
        8,        
		13,
        14,
        15,      
    ],
	DeviceIds: [
        1,
		2,
    ]
} ]);

// 7#道I列位
db.getCollection("ASIS_Position").insert([ {
    ID: 2,
    Index: 2,
    Name: "7#道I列位",
    OpcConfig: {
        // Address: "opc.tcp://192.168.2.30:4840",
        Address: "opc.tcp://10.6.1.30:4840",
        Username: "yd0484",
        Password: "yd123456"
    },
    VideoIds: [
        17,
        18,
        24,
		25,
        26,
        27
    ],
	ExtraVideoIds: [
        16,
        19,
        20,
        21,
        22,
        23,
        28,
           
    ],
	DeviceIds: [
        1,
		2,
    ]
} ]);

// 8#道I列位
db.getCollection("ASIS_Position").insert([ {
    ID: 3,
    Index: 3,
    Name: "8#道I列位",
    OpcConfig: {
        Address: "opc.tcp://10.6.1.45:4840",
        Username: "yd0484",
        Password: "yd123456"
    },
    VideoIds: [     
		58,
		59,
		65,
		66,
		67,
		68,
    ],
	ExtraVideoIds: [
		57,
		60,
		61,
		62,
		63,
		64,
		69,
		70,
    ],
	DeviceIds: [
        3,
		4,
    ]
} ]);

// 9#道I列位
db.getCollection("ASIS_Position").insert([ {
    ID: 4,
    Index: 4,
    Name: "9#道I列位",
    OpcConfig: {
        Address: "opc.tcp://10.6.1.60:4840",
        Username: "yd0484",
        Password: "yd123456"
    },
    VideoIds: [     
		73,
		74,
		80,
		81,
		82,
		83,
    ],
	ExtraVideoIds: [
		72,
		75,
		76,
		77,
		78,
		79,
		84,
		85,
    ],
	DeviceIds: [
        3,
		4,
    ]
} ]);

// 10#道I列位
db.getCollection("ASIS_Position").insert([ {
    ID: 5,
    Index: 5,
    Name: "10#道I列位",
    OpcConfig: {
        Address: "opc.tcp://10.6.1.73:4840",
        Username: "yd0484",
        Password: "yd123456"
    },
    VideoIds: [     
        30,
        31,
		37,
        38,
        39,
        40,
    ],
	ExtraVideoIds: [
        29,
        32,
        33,
		34,
        35,
        36,        
		41,
        42,
        43,      
    ],
	DeviceIds: [
        5,
		6,
    ]
} ]);

// 11#道I列位
db.getCollection("ASIS_Position").insert([ {
    ID: 6,
    Index: 6,
    Name: "11#道I列位",
    OpcConfig: {
        Address: "opc.tcp://10.6.1.88:4840",
        Username: "yd0484",
        Password: "yd123456"
    },
    VideoIds: [
        45,
        46,
        52,
		53,
        54,
        55
    ],
	ExtraVideoIds: [
        44,
        47,
        48,
		49,
        50,
        51,        
        56,      
    ],
	DeviceIds: [
        5,
		6,
    ]
} ]);

/* Other type of position
db.getCollection("ASIS_Position").insert([ {
    _id: ObjectId("615160bf3e330000f10025c2"),
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
} ]);
db.getCollection("ASIS_Position").insert([ {
    _id: ObjectId("615160e03e330000f10025c3"),
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
} ]);
db.getCollection("ASIS_Position").insert([ {
    _id: ObjectId("615160f43e330000f10025c4"),
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
} ]);
*/