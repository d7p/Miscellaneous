#ifndef SETTINGS_H
#define SETTINGS_H

#include <QObject>

/**
 * @brief The Settings class
 */
class Settings : public QObject
{
    Q_OBJECT
public:
    /**
     * @brief Settings Constructor
     * @param parent
     */
    explicit Settings(QObject *parent = 0);

    /**
     * @brief Settings Deconstuctor
     */
    ~Settings();

    /**
     * @brief GetPlayerName getter for playerName
     * @return
     */
    QString GetPlayerName();

    /**
     * @brief SetPlayerName setter for playName
     * @param playerName
     */
    void SetPlayerName(QString playerName);

    /**
     *@brief Settings::Loss updates total losses and resets the current streak
     *
     * Upadest the total losses  by decrementing by one and  resets the current streak
     *
     * @return
     */
    int Loss();

    /**
     * @brief Settings::Win updates the win streak and the win count
     *
     *  Upadest the total wins and current streak by incrementing them by one
     * @return
     */
    int Win();

    /**
     * @brief TotalGamesPlayed Getter
     * @return
     */
    int TotalGamesPlayed();

    /**
     * @brief TotalGamesWon Getter
     * @return
     */
    int TotalGamesWon();

    /**
     * @brief TotalGamesLost Getter
     * @return
     */
    int TotalGamesLost();

    /**
     * @brief workingDirectory
     */
    QString workingDirectory;

private:
    /**
     * @brief Load loads data form file
     *
     * Loads all the data and settings from settings.txt file
     */
    void Load();

    /**
     * @brief Save saves data to file
     *
     * Save all data and settings to settings.txt
     */
    void Save();

    /**
     * @brief wins
     */
    int wins;

    /**
     * @brief losses
     */
    int losses;

    /**
     * @brief streak
     */
    int streak;

    /**
     * @brief userName
     */
    QString userName;

signals:
    
public slots:
    
};

#endif // SETTINGS_H
