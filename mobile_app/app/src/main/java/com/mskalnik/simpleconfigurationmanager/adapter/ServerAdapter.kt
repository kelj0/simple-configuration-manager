package com.mskalnik.simpleconfigurationmanager.adapter

import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView
import com.mskalnik.simpleconfigurationmanager.R
import com.mskalnik.simpleconfigurationmanager.model.Server


class ServerAdapter (private val servers: List<Server>) : RecyclerView.Adapter<ServerAdapter.ViewHolder>() {
    inner class ViewHolder(listItemView: View) : RecyclerView.ViewHolder(listItemView) {
        val twServerName: TextView = itemView.findViewById(R.id.twServerName)
        val twServerStatus: TextView = itemView.findViewById(R.id.twServerStatus)
        val twServerIp: TextView = itemView.findViewById(R.id.twServerIp)
        val twServerOs: TextView = itemView.findViewById(R.id.twServerOs)
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): ServerAdapter.ViewHolder {
        val context = parent.context
        val inflater = LayoutInflater.from(context)
        val contactView = inflater.inflate(R.layout.fragment_server, parent, false)

        return ViewHolder(contactView)
    }

    override fun onBindViewHolder(viewHolder: ServerAdapter.ViewHolder, position: Int) {
        val contact: Server = servers[position]
        val twServerName = viewHolder.twServerName
        val twServerStatus = viewHolder.twServerStatus
        val twServerIp = viewHolder.twServerIp
        val twServerOs = viewHolder.twServerOs

        twServerName.text = contact.name
        twServerStatus.text = if (contact.online) "ONLINE" else "OFFLINE"
        twServerIp.text = contact.ip
        twServerOs.text = contact.lastOnline!!.toLocaleString()
    }

    override fun getItemCount(): Int {
        return servers.size
    }
}