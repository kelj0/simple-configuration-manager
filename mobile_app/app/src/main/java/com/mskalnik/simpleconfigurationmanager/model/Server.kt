package com.mskalnik.simpleconfigurationmanager.model

class Server(
    val idServer: Int,
    val serverName: String,
    val ipAddress: String,
    val isOnline: Boolean,
    var operatingSystemId: Int,
    val deleted: Boolean,
    var operatingSystem: String
) {}