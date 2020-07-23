package com.mskalnik.simpleconfigurationmanager

import android.content.Intent
import android.os.Bundle
import android.view.View
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import androidx.swiperefreshlayout.widget.SwipeRefreshLayout
import com.mskalnik.simpleconfigurationmanager.adapter.OnItemClickListener
import com.mskalnik.simpleconfigurationmanager.adapter.ServerAdapter
import com.mskalnik.simpleconfigurationmanager.adapter.addOnItemClickListener
import com.mskalnik.simpleconfigurationmanager.model.Server

class ServerListActivity : BaseActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.fragment_server_list)

        val rvServerList            = findViewById<RecyclerView>(R.id.rvServerList)
        val swipeRefreshLayout      = findViewById<SwipeRefreshLayout>(R.id.swContainer);
        val servers                 = Server.getServers()
        val adapter                 = ServerAdapter(servers)
        rvServerList.adapter        = adapter
        rvServerList.layoutManager  = LinearLayoutManager(this)

        rvServerList.addOnItemClickListener(object: OnItemClickListener {
            override fun onItemClicked(position: Int, view: View) {
                startActivity(Intent(applicationContext, ServerActivity::class.java))
            }
        })

        swipeRefreshLayout.setOnRefreshListener {
            recreate()
            swipeRefreshLayout.isRefreshing = false
        }
    }
}
