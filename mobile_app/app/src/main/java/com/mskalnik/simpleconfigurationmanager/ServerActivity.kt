package com.mskalnik.simpleconfigurationmanager

import android.os.Bundle
import android.widget.Button
import android.widget.TextView
import com.mskalnik.simpleconfigurationmanager.controller.ApiController
import com.mskalnik.simpleconfigurationmanager.model.Util

class ServerActivity : BaseActivity() {

    companion object {
        const val SERVER_ID_KEY         = "SERVER_ID";
        const val SERVER_NAME_KEY       = "SERVER_NAME";
        const val SERVER_STATUS_KEY     = "SERVER_STATUS";
        const val SERVER_IP_KEY         = "SERVER_IP";
        const val SERVER_DELETED_KEY    = "SERVER_DELETED";
        const val SERVER_OS_KEY         = "SERVER_OS";
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_server)
        initValues()
        setEventListeners()
    }

    private fun initValues() {
        val twServerIdValue: TextView       = findViewById(R.id.twServerIdValue)
        val twServerName: TextView          = findViewById(R.id.twServerNameValue)
        val twServerStatus: TextView        = findViewById(R.id.twServerStatusValue)
        val twServerIpAddress: TextView     = findViewById(R.id.twServerIpAddressValue)
        val twServerDeletedValue: TextView  = findViewById(R.id.twServerDeletedValue)
        val twServerOsLabel: TextView       = findViewById(R.id.twServerOsValue)

        twServerIdValue.text        = intent.getIntExtra(SERVER_ID_KEY, 0).toString()
        twServerName.text           = intent.getStringExtra(SERVER_NAME_KEY)
        twServerStatus.text         = if (intent.getBooleanExtra(SERVER_STATUS_KEY, false)) "ONLINE" else "OFFLINE"
        twServerIpAddress.text      = intent.getStringExtra(SERVER_IP_KEY)
        twServerDeletedValue.text   = if (intent.getBooleanExtra(SERVER_DELETED_KEY, false)) "Yes" else "No"
        twServerOsLabel.text        = intent.getStringExtra(SERVER_OS_KEY)
    }

    private fun setEventListeners() {
        val btnServerDelete: Button = findViewById(R.id.btnServerDelete)

        btnServerDelete.setOnClickListener {
            ApiController.removeServer(intent.getIntExtra(SERVER_ID_KEY, 0).toString())
            Util.showToast(this, getString(R.string.config_deleted))
            btnServerDelete.isEnabled = false;
        }
    }
}
