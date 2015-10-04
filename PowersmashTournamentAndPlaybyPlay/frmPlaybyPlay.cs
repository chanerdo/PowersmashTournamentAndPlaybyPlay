using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MySql.Data.MySqlClient;

namespace PowersmashTournamentAndPlaybyPlay
{
    public partial class frmPlaybyPlay : Form
    {

        private MySqlConnection connDB = new MySqlConnection("datasource=unicsoftworks.com;port=3306;username=admin;password=powersmash123;Convert Zero Datetime=true;");
        private DataTable user_data;
        private DataTable match_data;
        private Rectangle rect;
        private Image shuttle;
        private int player_id1, player_id2, player_id3, player_id4, matchtype, server, t1Score = 0, t2Score = 0, play_id = 0;

        private string tournamentname, team1, team2, player_name1, player_name2, player_name3, player_name4;

        public frmPlaybyPlay(string tourname, string t1, string t2, string pn1, string pn2, string pn3, string pn4, int mt, int pi1, int pi2, int pi3, int pi4)
        {
            matchtype = mt;
            if(matchtype == 2 || matchtype == 3)
            {
                tournamentname = tourname;
                team1 = t1;
                team2 = t2;
                player_name1 = pn1;
                player_name2 = pn2;
                player_name3 = pn3;
                player_name4 = pn4;
                player_id1 = pi1;
                player_id2 = pi2;
                player_id3 = pi3;
                player_id4 = pi4;
            }
            else if (matchtype == 1)
            {
                tournamentname = tourname;
                team1 = t1;
                team2 = t2;
                player_name1 = pn1;
                player_name2 = pn2;
                player_id1 = pi1;
                player_id2 = pi2;
            }
            InitializeComponent();
            lblTournamentName.Text = tournamentname;
        }

        private void frmPlaybyPlay_Load(object sender, EventArgs e)
        {
            getUserData();
            getMatchData();
            shuttle = Properties.Resources.shuttle;
            rect = new Rectangle(20, 20, 20, 20);
        }

        private void frmPlaybyPlay_Shown(object sender, EventArgs e)
        {
            if (matchtype == 2 || matchtype == 3)
            {
                byte[] image = null;
                MessageBox.Show(player_id1.ToString() + "\n" + player_id2.ToString());
                foreach (DataRow row in user_data.Rows)
                {
                    if (player_id1 == row.Field<int>(0))
                    {
                        image = (byte[])(row.Field<byte[]>(42));
                        MemoryStream memstream = new MemoryStream(image);
                        pbxMDPlayer1.Image = Image.FromStream(memstream);
                        lblMDPlayer1.Text = player_name1;
                    }
                    if (player_id2 == row.Field<int>(0))
                    {
                        image = (byte[])(row.Field<byte[]>(42));
                        MemoryStream memstream = new MemoryStream(image);
                        pbxMDPlayer2.Image = Image.FromStream(memstream);
                        lblMDPlayer2.Text = player_name2;
                    }
                    if (player_id3 == row.Field<int>(0))
                    {
                        image = (byte[])(row.Field<byte[]>(42));
                        MemoryStream memstream = new MemoryStream(image);
                        pbxMDPlayer3.Image = Image.FromStream(memstream);
                        lblMDPlayer3.Text = player_name3;
                    }
                    if (player_id4 == row.Field<int>(0))
                    {
                        image = (byte[])(row.Field<byte[]>(42));
                        MemoryStream memstream = new MemoryStream(image);
                        pbxMDPlayer4.Image = Image.FromStream(memstream);
                        lblMDPlayer4.Text = player_name4;
                    }
                }
                lblTeam1.Text = team1;
                lblTeam2.Text = team2;
                pnlMixDouble.Show();
                pnlSingle.Hide();
            }
            else if (matchtype == 1)
            {
                byte[] image = null;
                MessageBox.Show(player_id1.ToString() + "\n" + player_id2.ToString());
                foreach (DataRow row in user_data.Rows)
                {
                    if (player_id1 == row.Field<int>(0))
                    {
                        image = (byte[])(row.Field<byte[]>(42));
                        MemoryStream memstream = new MemoryStream(image);
                        pbxSPlayer1.Image = Image.FromStream(memstream);
                        lblSPlayer1.Text = player_name1;
                        lblTeam1.Text = team1;
                    }
                    if (player_id2 == row.Field<int>(0))
                    {
                        image = (byte[])(row.Field<byte[]>(42));
                        MemoryStream memstream = new MemoryStream(image);
                        pbxSPlayer2.Image = Image.FromStream(memstream);
                        lblSPlayer2.Text = player_name2;
                        lblTeam2.Text = team2;
                    }
                }
                pnlSingle.Show();
                pnlMixDouble.Hide();
                pbxSPlayer1.Click += new EventHandler(pbxSPlayer1_Click);
            }
        }

        private void getUserData()
        {
            try
            {
                if (connDB.State == ConnectionState.Open) { connDB.Close(); }
                connDB.Open();
                MySqlCommand cmdDB = new MySqlCommand("SELECT * FROM powersmash.user", connDB);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmdDB);
                user_data = new DataTable();
                sqlDataAdapter.Fill(user_data);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connDB.Close();
            }
        }

        private void getMatchData()
        {
            try
            {
                if (connDB.State == ConnectionState.Open) { connDB.Close(); }
                connDB.Open();
                MySqlCommand cmdDB = new MySqlCommand("SELECT * FROM powersmash.match", connDB);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmdDB);
                match_data = new DataTable();
                sqlDataAdapter.Fill(match_data);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connDB.Close();
            }
        }

        private void saveData(string query)
        {
            try
            {
                if (connDB.State == ConnectionState.Open) { connDB.Close(); }
                connDB.Open();
                MySqlCommand cmdDB = new MySqlCommand(query, connDB);
                cmdDB.ExecuteNonQuery();
            }
            catch (MySqlException ex) { MessageBox.Show(ex.Message); }
            finally
            {
                connDB.Close();
            }
        }

        private void pbxSCourt_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImage(shuttle, rect);
        }

        private void pbxSCourt_MouseClick(object sender, MouseEventArgs e)
        {
            rect = new Rectangle(e.X, e.Y, 20, 20);
            pbxSCourt.Invalidate();
            if (e.X >= 69 && e.X <= 327)
            {
                if (e.Y >= 45 && e.Y <= 272)
                {
                    t2Score++;
                    lblScore2.Text = t2Score.ToString("00");
                    foreach (DataRow row in match_data.Rows)
                    {
                        if (row.Field<int>(2) == player_id1 && row.Field<int>(3) == player_id2)
                        {
                            play_id++;
                            string query = "INSERT INTO powersmash.playbyplay (match_id, server, play_id, x_axis, y_axis, score_1, score_2)" +
                                           "VALUES ((SELECT id FROM powersmash.match WHERE player_1_1 = '" + player_id1 + "' AND player_1_2 = '" + player_id2 + "'),'" + server + "', '" + play_id + "', '" +
                                           e.X.ToString() + "', '" + e.Y.ToString() + "', '" + t1Score + "', '" + t2Score + "')";
                            saveData(query);
                        }
                    }
                }
            }
            if (e.X >= 327 && e.X <= 583)
            {
                if (e.Y >= 45 && e.Y <= 272)
                {
                    t1Score++;
                    lblScore1.Text = t1Score.ToString("00");
                    foreach (DataRow row in match_data.Rows)
                    {
                        if (row.Field<int>(2) == player_id1 && row.Field<int>(3) == player_id2)
                        {
                            play_id++;
                            string query = "INSERT INTO powersmash.playbyplay (match_id, server, play_id, x_axis, y_axis, score_1, score_2)" +
                                           "VALUES ((SELECT id FROM powersmash.match WHERE player_1_1 = '" + player_id1 + "' AND player_1_2 = '" + player_id2 + "'),'" + server + "', '" + play_id + "', '" +
                                           e.X.ToString() + "', '" + e.Y.ToString() + "', '" + t1Score + "', '" + t2Score + "')";
                            saveData(query);
                        }
                    }
                }
            }
        }

        private void pbxSPlayer1_Click(object sender, EventArgs e)
        {
            pbxSPlayer1.BorderStyle = BorderStyle.FixedSingle;
            pbxSPlayer2.BorderStyle = BorderStyle.None;
            server = player_id1;
        }

        private void pbxSPlayer2_Click(object sender, EventArgs e)
        {
            pbxSPlayer2.BorderStyle = BorderStyle.FixedSingle;
            pbxSPlayer1.BorderStyle = BorderStyle.None;
            server = player_id2;
        }

        private void pbxMDCourt_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImage(shuttle, rect);
        }

        private void pbxMDCourt_MouseClick(object sender, MouseEventArgs e)
        {
            rect = new Rectangle(e.X, e.Y, 20, 20);
            pbxMDCourt.Invalidate();
            if (e.X >= 35 && e.X <= 327)
            {
                if (e.Y >= 24 && e.Y <= 291)
                {
                    t2Score++;
                    lblScore2.Text = t2Score.ToString("00");
                    foreach (DataRow row in match_data.Rows)
                    {
                        if (row.Field<int>(2) == player_id1 && row.Field<int>(3) == player_id2)
                        {
                            play_id++;
                            string query = "INSERT INTO powersmash.playbyplay (match_id, server, play_id, x_axis, y_axis, score_1, score_2)" +
                                           "VALUES ((SELECT id FROM powersmash.match WHERE player_1_1 = '" + player_id1 + "' AND player_1_2 = '" + player_id2 + "' AND player_2_1 = '" + player_id3 +
                                           "' AND player_2_2 = '" + player_id4 + "'),'" + server + "', '" + play_id + "', '" + e.X.ToString() + "', '" + e.Y.ToString() + "', '" + t1Score + "', '" + t2Score + "')";
                            saveData(query);
                        }
                    }
                }
            }
            if (e.X >= 327 && e.X <= 617)
            {
                if (e.Y >= 24 && e.Y <= 291)
                {
                    t1Score++;
                    lblScore1.Text = t1Score.ToString("00");
                    foreach (DataRow row in match_data.Rows)
                    {
                        if (row.Field<int>(2) == player_id1 && row.Field<int>(3) == player_id2)
                        {
                            play_id++;
                            string query = "INSERT INTO powersmash.playbyplay (match_id, server, play_id, x_axis, y_axis, score_1, score_2)" +
                                           "VALUES ((SELECT id FROM powersmash.match WHERE player_1_1 = '" + player_id1 + "' AND player_1_2 = '" + player_id2 + "' AND player_2_1 = '" + player_id3 +
                                           "' AND player_2_2 = '" + player_id4 + "'),'" + server + "', '" + play_id + "','" + e.X.ToString() + "', '" + e.Y.ToString() + "', '" + t1Score + "', '" + t2Score + "')";
                            saveData(query);
                        }
                    }
                }
            }
        }
    }
}
