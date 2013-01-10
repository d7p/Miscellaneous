#include "settings.h"
#include <QFile>
#include <QTextStream>
#include <QStringList>

/**
 * @file settings.h
 * @brief Settings::Settings constructor
 * @param parent
 */
Settings::Settings(QObject *parent) :
    QObject(parent)
{
    Load();
}

/**
 * @brief Settings::~Settings deconstructor
 */
Settings::~Settings()
{
    //Save the current vars for next time
    Save();

}

/**
 * @brief Settings::GetPlayerName getter of PlayerName
 * @return
 */
QString Settings::GetPlayerName()
{
    return  userName.isEmpty()? userName: "User1";
}

/**
 * @brief Settings::SetPlayerName setter of PlayerName
 * @param playerName
 */
void Settings::SetPlayerName(QString playerName)
{
     userName=playerName;
}

/**
 * @brief Settings::Loss updates total losses and resets the current streak
 *
 * Upadest the total losses  by decrementing by one and  resets the current streak
 *
 * @return
 */
int Settings::Loss()
{
    streak = 0;
    losses++;
    return streak;
}

/**
 * @brief Settings::Win updates the win streak and the win count
 *
 *  Upadest the total wins and current streak by incrementing them by one
 *
 * @return the current win streak
 */
int Settings::Win()
{
    streak++;
    wins++;
    return streak;
}

/**
 * @brief Settings::TotalGamesPlayed Getter
 * @return
 */
int Settings::TotalGamesPlayed()
{
    return wins+losses;
}

/**
 * @brief Settings::TotalGamesWon Getter
 * @return
 */
int Settings::TotalGamesWon()
{
    return wins;
}

/**
 * @brief Settings::TotalGamesLost Getter
 * @return
 */
int Settings::TotalGamesLost()
{
    return losses;
}

/**
 * @brief Settings::Load loads settings
 *
 *Loads the setting from file for use latter these include:
 *wins
 *losses
 *streak
 *User Name
 */
void Settings::Load()
{
    //Used to hold the records till they are split lower down
   QString temp;

    //The file to be read from
    QFile file(workingDirectory+"/settings.txt");

    //Check if we can find and open the file else throw an error
   if (file.open(QIODevice::ReadOnly | QIODevice::Text))
   {
        QTextStream stream(&file);
        temp = stream.readLine();
        userName = stream.readLine();
        file.close();

        // split the line and place it in the right variables
         QStringList records;
         records = temp.split(",");
         wins = records.at(0).toInt();
         losses = records.at(1).toInt();
         streak = records.at(2).toInt();
   }
   else
   {
       wins = 0;
       losses = 0;
       streak = 0;
   }
}

/**
 * @brief Settings::Save save setting
 *
 *Saves the setting to settings.txt file for use later
 *
 */
void Settings::Save()
{
    // Get the current file path
    QFile file(workingDirectory+"/settings.txt");

    //Check if we can find and open the file else throw an error
   if (file.open(QIODevice::WriteOnly))
   {
    QTextStream stream(&file);
    //Output the string to file
    stream << userName<<"\n"<< wins<<","<<losses<<","<<streak;
   }
}
