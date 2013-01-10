#-------------------------------------------------
#
# Project created by QtCreator 2012-12-17T21:12:21
#
#-------------------------------------------------

QT       += core gui

greaterThan(QT_MAJOR_VERSION, 4): QT += widgets

TARGET = HangMan
TEMPLATE = app


SOURCES += main.cpp\
        mainwindow.cpp \
    model.cpp \
    settings.cpp

HEADERS  += mainwindow.h \
    model.h \
    settings.h

FORMS    += mainwindow.ui
