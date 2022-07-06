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

 Date: 18/05/2022 11:36:48
*/


// ----------------------------
// Collection structure for ASIS_ExternalSystem
// ----------------------------
db.getCollection("ASIS_ExternalSystem").drop();
db.createCollection("ASIS_ExternalSystem");

// ----------------------------
// Documents of ASIS_ExternalSystem
// ----------------------------
db.getCollection("ASIS_ExternalSystem").insert([ {
    _id: ObjectId("612856a8586700005c005d12"),
    ID: NumberInt("4"),
    Name: "宋和毅",
    AuthKey: "3333333333333333",
    Describe: "宋和毅",
    Enable: true,
    PushAddress: "http://192.168.1.100:9090/push"
} ]);
db.getCollection("ASIS_ExternalSystem").insert([ {
    _id: ObjectId("613f0565802000008a0078e2"),
    ID: NumberInt("6"),
    Name: "张亮",
    AuthKey: "5555555555555555",
    Enable: true,
    Describe: "张亮",
    PushAddress: "http://192.168.1.87:9090/push"
} ]);
db.getCollection("ASIS_ExternalSystem").insert([ {
    _id: ObjectId("614d3ce41b58000092001a92"),
    ID: NumberInt("7"),
    Name: "Shen12DP",
    AuthKey: "6666666666666666",
    Enable: true,
    Describe: "深12服务器",
    PushAddress: "http://192.168.1.102:8081/api/asis/push"
} ]);
db.getCollection("ASIS_ExternalSystem").insert([ {
    ID: NumberInt("8"),
    Name: "Test",
    AuthKey: "7777777777777777",
    Enable: true,
    Describe: "212测试专用",
    PushAddress: "http://192.168.1.212:9090/push"
} ]);

db.getCollection("ASIS_ExternalSystem").insert([ {
    ID: NumberInt("9"),
    Name: "BaiSe",
    AuthKey: "8888888888888888",
    Enable: true,
    Describe: "百色现场专用",
    PushAddress: "http://10.6.1.4:9090/push"
} ]);

db.getCollection("ASIS_ExternalSystem").insert([ {
    ID: NumberInt("10"),
    Name: "张亮BaiSe",
    AuthKey: "9999999999999999",
    Enable: true,
    Describe: "张亮百色现场专用",
    PushAddress: "http://10.6.1.87:9090/push"
} ]);