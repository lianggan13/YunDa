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

 Date: 06/07/2022 10:13:51
*/


// ----------------------------
// Collection structure for ASIS_Track
// ----------------------------
db.getCollection("ASIS_Track").drop();
db.createCollection("ASIS_Track");

// ----------------------------
// Documents of ASIS_Track
// ----------------------------
db.getCollection("ASIS_Track").insert([ {
    _id: ObjectId("62c4ecf5e3230000ac005af2"),
    ID: NumberInt("17"),
    No: NumberInt("17"),
    Name: "L17",
    Type: "TPT",
    PositionIds: [
        34
    ]
} ]);
db.getCollection("ASIS_Track").insert([ {
    _id: ObjectId("62c4ecf5e3230000ac005af3"),
    ID: NumberInt("18"),
    No: NumberInt("18"),
    Name: "L18",
    Type: "TPT",
    PositionIds: [
        36
    ]
} ]);
db.getCollection("ASIS_Track").insert([ {
    _id: ObjectId("62c4ecf5e3230000ac005af4"),
    ID: NumberInt("19"),
    No: NumberInt("19"),
    Name: "L19",
    Type: "TPT",
    PositionIds: [
        38
    ]
} ]);
db.getCollection("ASIS_Track").insert([ {
    _id: ObjectId("62c4ecf5e3230000ac005af5"),
    ID: NumberInt("20"),
    No: NumberInt("20"),
    Name: "L20",
    Type: "TPT",
    PositionIds: [
        40
    ]
} ]);
db.getCollection("ASIS_Track").insert([ {
    _id: ObjectId("62c4ecf5e3230000ac005af6"),
    ID: NumberInt("21"),
    No: NumberInt("21"),
    Name: "L21",
    Type: "TPT",
    PositionIds: [
        41,
        42
    ]
} ]);
db.getCollection("ASIS_Track").insert([ {
    _id: ObjectId("62c4ecf5e3230000ac005af7"),
    ID: NumberInt("22"),
    No: NumberInt("22"),
    Name: "L22",
    Type: "TPT",
    PositionIds: [
        43,
        44
    ]
} ]);
db.getCollection("ASIS_Track").insert([ {
    _id: ObjectId("62bc1f44bc0800003a001206"),
    ID: NumberInt("24"),
    No: NumberInt("24"),
    Name: "L24",
    Type: "TPT",
    PositionIds: [
        45
    ]
} ]);
db.getCollection("ASIS_Track").insert([ {
    _id: ObjectId("62bc1f44bc0800003a001207"),
    ID: NumberInt("25"),
    No: NumberInt("25"),
    Name: "L25",
    Type: "TPT",
    PositionIds: [
        46
    ]
} ]);
db.getCollection("ASIS_Track").insert([ {
    _id: ObjectId("62bc1f44bc0800003a001208"),
    ID: NumberInt("26"),
    No: NumberInt("26"),
    Name: "L26",
    Type: "TPT",
    PositionIds: [
        47
    ]
} ]);
db.getCollection("ASIS_Track").insert([ {
    _id: ObjectId("62bc1f44bc0800003a001209"),
    ID: NumberInt("27"),
    No: NumberInt("27"),
    Name: "L27",
    Type: "TPT",
    PositionIds: [
        48
    ]
} ]);
