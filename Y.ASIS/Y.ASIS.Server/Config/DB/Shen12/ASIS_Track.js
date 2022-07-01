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


db.getCollection("ASIS_Track").insert([ {
    ID: NumberInt("24"),
    No: NumberInt("24"),
    Name: "L24",
    Type: "TPT",
    PositionIds: [
        45
    ]
} ]);


db.getCollection("ASIS_Track").insert([ {
    ID: NumberInt("25"),
    No: NumberInt("25"),
    Name: "L25",
    Type: "TPT",
    PositionIds: [
        46
    ]
} ]);

db.getCollection("ASIS_Track").insert([ {
    ID: NumberInt("26"),
    No: NumberInt("26"),
    Name: "L26",
    Type: "TPT",
    PositionIds: [
        47
    ]
} ]);

db.getCollection("ASIS_Track").insert([ {
    ID: NumberInt("27"),
    No: NumberInt("27"),
    Name: "L27",
    Type: "TPT",
    PositionIds: [
        48
    ]
} ]);

