# screenplay
A game script is a text written for video game development, describing the plot, characters, dialogues, and interactive elements.

## version 1.1.1
add: 新增mainwindow,新增page block和text block
add: page block支持添加/删除text block
add: 添加empty template,只有显隐功能

## version 1.2.0
add: 聚焦时显示plaintext
mod: 通过聚焦方式去获取当前block
add: 方向键up/down切换聚焦block
add: 回车键创建新的block，同时需要确定父子关系树
add: tab键缩进block,修改父子关系树
add: 支持删除block

## version 1.3.0
add: 新增meta resource类用来存储数据,新增工厂类用来创建block
mod: 去掉多余侧边栏，改为mianwindow创建page block
add: 拆分window和page，支持添加/删除page
