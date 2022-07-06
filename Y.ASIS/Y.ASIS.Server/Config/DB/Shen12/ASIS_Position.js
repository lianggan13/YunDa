/*
 Navicat Premium Data Transfer

 Source Server         : 127.0.0.1
 Source Server Type    : MongoDB
 Source Server Version : 40414
 Source Host           : localhost:27017
 Source Schema         : ASIS

 Target Server Type    : MongoDB
 Target Server Version : 40414
 File Encoding         : 65001

 Date: 06/07/2022 10:14:03
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
    _id: ObjectId("62c13a36be7900000a007c87"),
    ID: 34,
    Index: 34,
    Name: "B",
    OpcConfig: {
        Address: "opc.tcp://10.60.17.2:4840",
        Username: "yd0484",
        Password: "yd123456"
    },
    VideoIds: [ ],
    ExtraVideoIds: [ ],
    DeviceIds: [ ]
} ]);
db.getCollection("ASIS_Position").insert([ {
    _id: ObjectId("62c4ed62e3230000ac005aff"),
    ID: 36,
    Index: 36,
    Name: "B",
    OpcConfig: {
        Address: "opc.tcp://10.60.18.2:4840",
        Username: "yd0484",
        Password: "yd123456"
    },
    VideoIds: [ ],
    ExtraVideoIds: [ ],
    DeviceIds: [ ]
} ]);
db.getCollection("ASIS_Position").insert([ {
    _id: ObjectId("62c4ed62e3230000ac005b00"),
    ID: 38,
    Index: 38,
    Name: "B",
    OpcConfig: {
        Address: "opc.tcp://10.60.19.2:4840",
        Username: "yd0484",
        Password: "yd123456"
    },
    VideoIds: [ ],
    ExtraVideoIds: [ ],
    DeviceIds: [ ]
} ]);
db.getCollection("ASIS_Position").insert([ {
    _id: ObjectId("62c4ed62e3230000ac005b01"),
    ID: 40,
    Index: 40,
    Name: "B",
    OpcConfig: {
        Address: "opc.tcp://10.60.20.2:4840",
        Username: "yd0484",
        Password: "yd123456"
    },
    VideoIds: [ ],
    ExtraVideoIds: [ ],
    DeviceIds: [ ]
} ]);
db.getCollection("ASIS_Position").insert([ {
    _id: ObjectId("62c4ed62e3230000ac005b02"),
    ID: 41,
    Index: 41,
    Name: "A",
    OpcConfig: {
        Address: "opc.tcp://10.60.21.2:4840",
        Username: "yd0484",
        Password: "yd123456"
    },
    VideoIds: [ ],
    ExtraVideoIds: [ ],
    DeviceIds: [ ]
} ]);
db.getCollection("ASIS_Position").insert([ {
    _id: ObjectId("62c4ed62e3230000ac005b03"),
    ID: 42,
    Index: 42,
    Name: "B",
    OpcConfig: {
        Address: "opc.tcp://10.60.21.62:4840",
        Username: "yd0484",
        Password: "yd123456"
    },
    VideoIds: [ ],
    ExtraVideoIds: [ ],
    DeviceIds: [ ]
} ]);
db.getCollection("ASIS_Position").insert([ {
    _id: ObjectId("62c4ed62e3230000ac005b04"),
    ID: 43,
    Index: 43,
    Name: "A",
    OpcConfig: {
        Address: "opc.tcp://10.60.22.2:4840",
        Username: "yd0484",
        Password: "yd123456"
    },
    VideoIds: [ ],
    ExtraVideoIds: [ ],
    DeviceIds: [ ]
} ]);
db.getCollection("ASIS_Position").insert([ {
    _id: ObjectId("62c4ed62e3230000ac005b05"),
    ID: 44,
    Index: 44,
    Name: "B",
    OpcConfig: {
        Address: "opc.tcp://10.60.22.62:4840",
        Username: "yd0484",
        Password: "yd123456"
    },
    VideoIds: [ ],
    ExtraVideoIds: [ ],
    DeviceIds: [ ]
} ]);
db.getCollection("ASIS_Position").insert([ {
    _id: ObjectId("62c4ed62e3230000ac005b06"),
    ID: 45,
    Index: 45,
    Name: "A",
    OpcConfig: {
        Address: "opc.tcp://10.60.25.2:4840",
        Username: "yd0484",
        Password: "yd123456"
    },
    VideoIds: [ ],
    ExtraVideoIds: [ ],
    DeviceIds: [ ]
} ]);
db.getCollection("ASIS_Position").insert([ {
    _id: ObjectId("62c4ed62e3230000ac005b07"),
    ID: 46,
    Index: 46,
    Name: "A",
    OpcConfig: {
        Address: "opc.tcp://10.60.25.2:4840",
        Username: "yd0484",
        Password: "yd123456"
    },
    VideoIds: [ ],
    ExtraVideoIds: [ ],
    DeviceIds: [
        1
    ]
} ]);
db.getCollection("ASIS_Position").insert([ {
    _id: ObjectId("62c4ed62e3230000ac005b08"),
    ID: 47,
    Index: 47,
    Name: "A",
    OpcConfig: {
        Address: "opc.tcp://10.60.26.2:4840",
        Username: "yd0484",
        Password: "yd123456"
    },
    VideoIds: [ ],
    ExtraVideoIds: [ ],
    DeviceIds: [
        2,
        3
    ]
} ]);
db.getCollection("ASIS_Position").insert([ {
    _id: ObjectId("62c4ed62e3230000ac005b09"),
    ID: 48,
    Index: 48,
    Name: "A",
    OpcConfig: {
        Address: "opc.tcp://10.60.27.2:4840",
        Username: "yd0484",
        Password: "yd123456"
    },
    VideoIds: [ ],
    ExtraVideoIds: [ ],
    DeviceIds: [
        4,
        5
    ]
} ]);
