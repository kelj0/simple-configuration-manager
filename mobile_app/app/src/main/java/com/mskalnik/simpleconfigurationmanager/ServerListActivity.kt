package com.mskalnik.simpleconfigurationmanager

import android.os.Bundle
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import androidx.swiperefreshlayout.widget.SwipeRefreshLayout
import com.mskalnik.simpleconfigurationmanager.adapter.ServerListAdapter
import com.mskalnik.simpleconfigurationmanager.model.Server

class ServerListActivity : BaseActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.fragment_server_list)

        val rvServerList            = findViewById<RecyclerView>(R.id.rvServerList)
        val swipeRefreshLayout      = findViewById<SwipeRefreshLayout>(R.id.swContainer);
        val servers                 = Server.getServers()
        val adapter                 = ServerListAdapter(servers)
        rvServerList.adapter        = adapter
        rvServerList.layoutManager  = LinearLayoutManager(this)

        swipeRefreshLayout.setOnRefreshListener {
            recreate()
            swipeRefreshLayout.isRefreshing = false
        }
    }
}
