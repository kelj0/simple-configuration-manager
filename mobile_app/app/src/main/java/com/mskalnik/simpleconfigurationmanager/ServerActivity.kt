package com.mskalnik.simpleconfigurationmanager

import android.os.Bundle
import android.widget.TextView

class ServerActivity : BaseActivity() {

    companion object {
        const val SERVER_NAME_KEY   = "SERVER_NAME";
        const val SERVER_STATUS_KEY = "SERVER_STATUS";
        const val SERVER_IP_KEY     = "SERVER_IP";
        const val SERVER_OS_KEY     = "SERVER_OS";
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_server)
        initValues()
    }

    private fun initValues() {
        val twServerName: TextView      = findViewById(R.id.twServerNameValue)
        val twServerStatus: TextView    = findViewById(R.id.twServerStatusValue)
        val twServerIpAddress: TextView = findViewById(R.id.twServerIpAddressValue)
        val twServerOsLabel: TextView   = findViewById(R.id.twServerOsValue)

        twServerName.text       = intent.getStringExtra(SERVER_NAME_KEY)
        twServerStatus.text     = if (intent.getBooleanExtra(SERVER_STATUS_KEY, false)) "ONLINE" else "OFFLINE"
        twServerIpAddress.text  = intent.getStringExtra(SERVER_IP_KEY)
        twServerOsLabel.text    = intent.getStringExtra(SERVER_OS_KEY)
    }
}
