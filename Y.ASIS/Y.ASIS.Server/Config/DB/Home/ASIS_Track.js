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

// 一股道
db.getCollection("ASIS_Track").insert([ {
    ID: NumberInt("1"),
    No: NumberInt("1"),
    Name: "一股道",
    Type: "TPTT",
    PositionIds: [
        1
    ]
} ]);

// 二股道
db.getCollection("ASIS_Track").insert([ {
    ID: NumberInt("2"),
    No: NumberInt("7"),
    Name: "二股道",
    Type: "TPT",
    PositionIds: [
        
    ]
} ]);

// 三股道
db.getCollection("ASIS_Track").insert([ {
    ID: NumberInt("3"),
    No: NumberInt("8"),
    Name: "三股道",
    Type: "TPTT",
    PositionIds: [
        
    ]
} ]);

