package com.mskalnik.simpleconfigurationmanager

import android.Manifest
import android.content.Intent
import android.content.pm.PackageManager
import android.os.Bundle
import android.os.PersistableBundle
import android.view.Menu
import android.view.MenuInflater
import android.view.MenuItem
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import androidx.core.content.ContextCompat
import java.util.*


open class BaseActivity : AppCompatActivity() {

    override fun onCreate(savedInstanceState: Bundle?, persistentState: PersistableBundle?) {
        loadLocale();
        super.onCreate(savedInstanceState, persistentState)
    }

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
            R.id.menuWeb -> {
                startActivity(Intent(this, WelcomeActivity::class.java))
                true
            }
            else -> super.onOptionsItemSelected(item)
        }
    }

    private fun loadLocale() {
        val shp = getSharedPreferences(
            "com.mskalnik.simpleconfigurationmanager.PREFERENCES",
            MODE_PRIVATE
        )
        val language = shp.getString("LANGUAGE", null)
        changeLang(language)
    }

    open fun changeLang(language: String?) {
        if (language == null) return
        saveLocale(language);

        val locale = Locale(language)
        Locale.setDefault(Locale(language))

        val configuration = resources.configuration
        configuration.setLocale(locale)
        applicationContext.resources.updateConfiguration(configuration, resources.displayMetrics)
        recreate();
    }

    private fun saveLocale(language: String) {
        val sharedPreferences = getSharedPreferences(
            "com.mskalnik.simpleconfigurationmanager.PREFERENCES",
            MODE_PRIVATE
        )
        val editor = sharedPreferences.edit()
        editor.putString("LANGUAGE", language).apply()
    }
}