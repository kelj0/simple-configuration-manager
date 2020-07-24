package com.mskalnik.simpleconfigurationmanager.adapter

import android.graphics.Color
import android.view.LayoutInflater
import android.view.ViewGroup
import androidx.recyclerview.widget.RecyclerView
import com.mskalnik.simpleconfigurationmanager.R
import com.mskalnik.simpleconfigurationmanager.model.Server


class ServerListAdapter(private val servers: List<Server>) : RecyclerView.Adapter<ServerListViewHolder>() {
    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): ServerListViewHolder {
        val context     = parent.context
        val inflater    = LayoutInflater.from(context)
        val serverView  = inflater.inflate(R.layout.fragment_server, parent, false)

        return ServerListViewHolder(serverView, servers)
    }

    override fun onBindViewHolder(viewHolder: ServerListViewHolder, position: Int) {
        val server: Server      = servers[position]
        val twServerName        = viewHolder.twServerName
        val twServerStatus      = viewHolder.twServerStatus
        val twServerIp          = viewHolder.twServerIp
        val twServerOs          = viewHolder.twServerOs

        twServerName.text       = server.serverName
        twServerStatus.text     = if (server.isOnline) "ONLINE" else "OFFLINE"
        twServerIp.text         = server.ipAddress
        twServerOs.text         = server.operatingSystem

        if (server.isOnline) twServerStatus.setBackgroundColor(Color.GREEN)
        else twServerStatus.setBackgroundColor(Color.RED)
    }

    override fun getItemCount(): Int {
        return servers.size
    }
}