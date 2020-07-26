package com.mskalnik.simpleconfigurationmanager.adapter

import android.content.Intent
import android.view.View
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView
import com.mskalnik.simpleconfigurationmanager.R
import com.mskalnik.simpleconfigurationmanager.ServerActivity
import com.mskalnik.simpleconfigurationmanager.ServerActivity.Companion.SERVER_DELETED_KEY
import com.mskalnik.simpleconfigurationmanager.ServerActivity.Companion.SERVER_ID_KEY
import com.mskalnik.simpleconfigurationmanager.ServerActivity.Companion.SERVER_IP_KEY
import com.mskalnik.simpleconfigurationmanager.ServerActivity.Companion.SERVER_NAME_KEY
import com.mskalnik.simpleconfigurationmanager.ServerActivity.Companion.SERVER_OS_KEY
import com.mskalnik.simpleconfigurationmanager.ServerActivity.Companion.SERVER_STATUS_KEY
import com.mskalnik.simpleconfigurationmanager.model.Server

class ServerListViewHolder(view: View, servers: List<Server>) : RecyclerView.ViewHolder(view) {
    val twServerName: TextView      = view.findViewById(R.id.twServerName)
    val twServerStatus: TextView    = view.findViewById(R.id.twServerStatus)
    val twServerIp: TextView        = view.findViewById(R.id.twServerIp)
    val twServerOs: TextView        = view.findViewById(R.id.twServerOs)

    init {
        view.setOnClickListener{
            val intent = Intent(view.context, ServerActivity::class.java)

            intent.putExtra(SERVER_ID_KEY, servers[adapterPosition].idServer)
            intent.putExtra(SERVER_NAME_KEY, servers[adapterPosition].serverName)
            intent.putExtra(SERVER_STATUS_KEY, servers[adapterPosition].isOnline)
            intent.putExtra(SERVER_IP_KEY, servers[adapterPosition].ipAddress)
            intent.putExtra(SERVER_DELETED_KEY, servers[adapterPosition].deleted)
            intent.putExtra(SERVER_OS_KEY, servers[adapterPosition].operatingSystem)

            view.context.startActivity(intent);
        }
    }
}