package com.mskalnik.simpleconfigurationmanager

import android.content.Intent
import android.content.res.Configuration
import android.content.res.Resources
import android.util.DisplayMetrics
import android.view.Menu
import android.view.MenuInflater
import android.view.MenuItem
import androidx.appcompat.app.AppCompatActivity
import java.util.*


open class BaseActivity : AppCompatActivity() {

    override fun onCreateOptionsMenu(menu: Menu): Boolean {
        val inflater: MenuInflater = menuInflater
        inflater.inflate(R.menu.menu_toolbar, menu)
        return true
    }

    override fun onOptionsItemSelected(item: MenuItem): Boolean {
        return when (item.itemId) {
            R.id.menuCroatian -> {
                changeLang("hr")
                true
            }
            R.id.menuEnglish -> {
                changeLang("en")
                true
            }
            R.id.menuLogout -> {
                startActivity(Intent(this, WelcomeActivity::class.java))
                true
            }
            else -> super.onOptionsItemSelected(item)
        }
    }

    private fun changeLang(locale: String) {
        val resources: Resources    = resources
        val dm: DisplayMetrics      = resources.displayMetrics
        val config: Configuration   = resources.configuration

        config.setLocale(Locale(locale.toLowerCase()))
        resources.updateConfiguration(config, dm)
        recreate()
    }
}