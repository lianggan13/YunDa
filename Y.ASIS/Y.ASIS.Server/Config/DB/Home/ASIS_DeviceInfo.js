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

 Date: 30/05/2022 14:41:59
*/


// ----------------------------
// Collection structure for ASIS_DeviceInfo
// ----------------------------
db.getCollection("ASIS_DeviceInfo").drop();
db.createCollection("ASIS_DeviceInfo");

// ----------------------------
// Documents of ASIS_DeviceInfo
// ----------------------------
db.getCollection("ASIS_DeviceInfo").insert([{
    ID: NumberInt("1"),
    Name: "刷脸机(销权)",
    Type: "Attendance",
    Ip: "192.168.1.126",
    Extension: "{\"Type\":\"Revoke\"}"
}]);
db.getCollection("ASIS_DeviceInfo").insert([{
    ID: NumberInt("2"),
    Name: "音柱",
    Type: "Speaker",
    Ip: "192.168.1.110",
    Port: 5060,
    Extension: "{\"TerminalId\":1}"
}]);
db.getCollection("ASIS_DeviceInfo").insert([{
    ID: NumberInt("3"),
    Name: "股道切换板卡#1",
    Type: "Switcher",
    Ip: "192.168.2.51",
    Port: 502,
    Extension: "{\"StartIndex\":1, \"Count\":6}"
}]);
db.getCollection("ASIS_DeviceInfo").insert([{
    ID: NumberInt("4"),
    Name: "股道切换板卡#2",
    Type: "Switcher",
    Ip: "192.168.2.52",
    Port: 502,
    Extension: "{\"StartIndex\":1, \"Count\":6}"
}]);

