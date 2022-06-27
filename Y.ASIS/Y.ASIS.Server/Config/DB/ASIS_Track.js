/*
 Navicat Premium Data Transfer

 Source Server         : LocalMongodb
 Source Server Type    : MongoDB
 Source Server Version : 40400
 Source Host           : localhost:27017
 Source Schema         : ASIS

 Target Server Type    : MongoDB
 Target Server Version : 40400
 File Encoding         : 65001

 Date: 18/05/2022 14:16:12
*/


// ----------------------------
// Collection structure for ASIS_Track
// ----------------------------
db.getCollection("ASIS_Track").drop();
db.createCollection("ASIS_Track");

// ----------------------------
// Documents of ASIS_Track
// ----------------------------

// 六股道
db.getCollection("ASIS_Track").insert([ {
    ID: NumberInt("1"),
    No: NumberInt("6"),
    Name: "六股道",
    Type: "TPTT",
    PositionIds: [
        1
    ]
} ]);

// 七股道
db.getCollection("ASIS_Track").insert([ {
    ID: NumberInt("2"),
    No: NumberInt("7"),
    Name: "七股道",
    Type: "TPTT",
    PositionIds: [
        2
    ]
} ]);

// 八股道
db.getCollection("ASIS_Track").insert([ {
    ID: NumberInt("3"),
    No: NumberInt("8"),
    Name: "八股道",
    Type: "TPTT",
    PositionIds: [
        3
    ]
} ]);

// 九股道
db.getCollection("ASIS_Track").insert([ {
    ID: NumberInt("4"),
    No: NumberInt("9"),
    Name: "九股道",
    Type: "TPTT",
    PositionIds: [
        4
    ]
} ]);

// 十股道
db.getCollection("ASIS_Track").insert([ {
    ID: NumberInt("5"),
    No: NumberInt("10"),
    Name: "十股道",
    Type: "TPTT",
    PositionIds: [
        5
    ]
} ]);

// 十一股道
db.getCollection("ASIS_Track").insert([ {
    ID: NumberInt("6"),
    No: NumberInt("11"),
    Name: "十一股道",
    Type: "TPTT",
    PositionIds: [
        6
    ]
} ]);


/* Other type of track
db.getCollection("ASIS_Track").insert([ {
    _id: ObjectId("610b522c65220000ee000fa4"),
    ID: NumberInt("2"),
    No: NumberInt("2"),
    Name: "二股道",
    Type: "TPT",
    PositionIds: [
        
    ]
} ]);
db.getCollection("ASIS_Track").insert([ {
    _id: ObjectId("610b524065220000ee000fa5"),
    ID: NumberInt("3"),
    No: NumberInt("3"),
    Name: "三股道",
    Type: "TPPTT",
    PositionIds: [
        
       
    ]
} ]);
*/