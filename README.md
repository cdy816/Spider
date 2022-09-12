**Internet of things Troika：[Mars realtime database](https://github.com/cdy816/mars) 、Acquisition platform [Spider](https://github.com/cdy816/Spider) 、Cross platform UI solution [Chameleon](https://github.com/cdy816/Chameleon)、Data alarm&analysis engine [ant](https://github.com/cdy816/Ant)**    

**>>>>[中文](https://github.com/cdy816/Spider/blob/master/README.zh-CN.md)>>>>**
 <br />
 <br />
[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![Apache License][license-shield]][license-url]
<br />
[![star](https://gitee.com/chongdaoyang/Spider/badge/star.svg?theme=white)](https://gitee.com/chongdaoyang/Spider/stargazers)
[![fork](https://gitee.com/chongdaoyang/Spider/badge/fork.svg?theme=white)](https://gitee.com/chongdaoyang/Spider/members)

<!-- PROJECT LOGO -->
<br />
<p align="center">

  <h2 align="center">Spider data interconnection</h2>
 
  <p align="center">
    High performance, cross platform equipment Internet of things collection platform!      
    <br />
    <br />
    <a href="https://github.com/cdy816/Spider/tree/master/Doc">document</a>
    ·
    <a href="https://github.com/cdy816/Spider/issues">Bug submission</a>
    ·
    <a href="https://github.com/cdy816/Spider/issues">Function application</a>
  </p>
</p>

# Spider
cross platform(Window、Linux、Iot) equipment Internet of things collection platform.Spider can be used with Mars as the device acquisition module of the database; At the same time, it can also run independently to match with other real-time database software.

1. Spider supports the development of multiple projects, and each project is a set of independent physical device acquisition units.
2. Multiple projects can be cascaded in a tree structure to support data aggregation
3. Supports computational devices and variables that allow users to implement specific engineering logic by embedding their own c# script code (provides direct access to standard or customized .net class libraries).
4. Resolution of device protocol through embedded c# script (provides direct access to standard or customized .net class libraries).

## List of supporting protocols
1. MQTT
2. OPC UA
3. Modbus
4. Coap
5. Siements plc
6. AllenBradley plc
7. Fatek plc
8. Fuju plc
9. Melsec plc
10. Omron plc
11. Panasonic plc
12. Keyence plc
13. Lsis plc

## Runtime Environment
The system platform is developed on. Net 5 platform, which can run on desktop system and embedded devices (such as raspberry pi)
* runtime system：It can be deployed in windows, Linux, IOT and other operating systems, or in docker. 
* development system：It adopts graphical configuration interface and CS structure, and is tentatively implemented in desktop mode, running in window system.

## Quick get start document&development document
1. [Document](https://github.com/cdy816/Spider/blob/master/Doc)

## Rlease
* [V0.6](https://github.com/cdy816/Spider/releases/tag/V0.6)


<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/cdy816/spider.svg?style=for-the-badge
[contributors-url]: https://github.com/cdy816/mars/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/cdy816/spider.svg?style=for-the-badge
[forks-url]:https://github.com/cdy816/mars/network/members
[stars-shield]: https://img.shields.io/github/stars/cdy816/spider.svg?style=for-the-badge
[stars-url]:https://github.com/cdy816/mars/stargazers
[issues-shield]: https://img.shields.io/github/issues/cdy816/spider.svg?style=for-the-badge
[issues-url]:https://github.com/cdy816/mars/issues
[license-shield]: https://img.shields.io/github/license/cdy816/spider.svg?style=for-the-badge
[license-url]: https://github.com/cdy816/spider/blob/master/LICENSE
