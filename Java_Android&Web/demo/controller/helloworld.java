package com.helloworld.demo.controller;

import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController

public class HelloWorldController{
    @RequestMapping("/SayHi")
    private String sayHello(){
        return "Hello World";
    }
}