using FligthApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace FligthApp.Context
{
    public class BaseContext 
    {
        public string str;
        public SqlConnection myConn = new SqlConnection(@"Server=(localdb)\MSSQLLocalDB;Integrated Security=true;"); 
        public void Initial()
        {
            str = "if(not exists(select name from master.dbo.sysdatabases where ('[' + name + ']' = 'FligthDb' or name	= 'FligthDb')))" +
                                "BEGIN;" +
                                "CREATE DATABASE FligthDb;" +
                                "END;";

            SqlCommand myCommand = new SqlCommand(str, myConn);

            try
            {
                myConn.Open();
                myCommand.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                ex.ToString();
            }
            finally
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
        }

        public void Create()
        {
            str = "if(exists(select name from master.dbo.sysdatabases where ('[' + name + ']' = 'FligthDb' or name	= 'FligthDb')))" +
                                "BEGIN;" +
                                "USE FligthDb;" +
                                "CREATE TABLE Airplane (" +
                                    "Id int not null IDENTITY(1,1) PRIMARY KEY," +
                                    "Nome varchar(200) not null," +
                                    "Marca varchar(200)" +
                                ");" +
                                "CREATE TABLE Passanger (" +
                                    "Id int not null IDENTITY(1,1) PRIMARY KEY," +
                                    "Nome varchar(200) not null," +
                                    "Sobrenome varchar(200)," +
                                    "Email varchar(200)," +
                                    "AirplaneId int FOREIGN KEY REFERENCES Airplane(Id)" +
                                ");" +
                                "END;";

            SqlCommand myCommand = new SqlCommand(str, myConn);

            try
            {
                myConn.Open();
                myCommand.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                ex.ToString();
            }
            finally
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
        }

        public void InsertPassangerToAirplane(int idPassanger, int idAirplane)
        {
            str = "UPDATE FligthDb.Passanger SET AirplaneId = @idAirplane WHERE Id = @passangerId;";

            SqlCommand command = new SqlCommand(str, myConn);
            command.Parameters.Add("@passangerId", SqlDbType.Int);
            command.Parameters["@passangerId"].Value = idPassanger;
            command.Parameters.Add("@idAirplane", SqlDbType.Int);
            command.Parameters["@idAirplane"].Value = idAirplane;

            try
            {
                myConn.Open();
                Int32 rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine("Linhas afetadas: {0}", rowsAffected);
            }
            catch (System.Exception ex)
            {
                ex.ToString();
            }
            finally
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
        }

        public bool InsertAirplane(Airplane airplane)
        {
            str = "USE FligthDB;" +
                "INSERT INTO Airplane (Nome, Marca) VALUES (@Nome, @Marca)";
            SqlCommand command;

            using(SqlConnection connection = myConn)
            {
                command = new SqlCommand(str, connection);
                command.Parameters.Add("@Nome", SqlDbType.Text);
                command.Parameters["@Nome"].Value = airplane.Nome;
                command.Parameters.Add("@Marca", SqlDbType.Text);
                command.Parameters["@Marca"].Value = airplane.Marca;

                try
                {
                    connection.Open();
                    var linhasAfetadas = command.ExecuteNonQuery();
                    Console.WriteLine("Linhas afetadas: {0}", linhasAfetadas);
                    return true;
                } catch(Exception ex)
                {
                    ex.ToString();
                    return false;
                }
            }
        }

        public bool InsertPassanger(Passanger passanger)
        {
            str = "USE FligthDB;" +
                    "INSERT INTO Passanger (Nome, Sobrenome, Email, AirplaneId) VALUES (@Nome, @Sobrenome, @Email, @AirplaneId)";
            SqlCommand command;

            using (SqlConnection connection = myConn)
            {
                command = new SqlCommand(str, connection);
                command.Parameters.Add("@Nome", SqlDbType.Text);
                command.Parameters["@Nome"].Value = passanger.Nome;
                command.Parameters.Add("@Sobrenome", SqlDbType.Text);
                command.Parameters["@Sobrenome"].Value = passanger.Sobrenome;
                command.Parameters.Add("@Email", SqlDbType.Text);
                command.Parameters["@Email"].Value = passanger.Email;
                command.Parameters.Add("@AirplaneId", SqlDbType.Int);
                command.Parameters["@AirplaneId"].Value = passanger.AirplaneId;

                try
                {
                    connection.Open();
                    var linhasAfetadas = command.ExecuteNonQuery();
                    Console.WriteLine("Linhas afetadas: {0}", linhasAfetadas);
                    return true;
                }
                catch (Exception ex)
                {
                    ex.ToString();
                    return false;
                }
            }
        }

        public List<Passanger> ListAllPassangerByAirplane(int idAirplane)
        {
            str = "USE FligthDB;" +
                   "SELECT * FROM Passanger WHERE AirplaneId = @idAirplane;";
            SqlCommand command;
            List<Passanger> passangers = new List<Passanger>();
            Passanger passanger;

            using (SqlConnection connection = myConn)
            {
                command = new SqlCommand(str, connection);
                command.Parameters.Add("@idAirplane", SqlDbType.Int);
                command.Parameters["@idAirplane"].Value = idAirplane;
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            passanger = new Passanger();
                            passanger.Id = reader.GetInt32(0);
                            passanger.Nome = reader.GetString(1);
                            passanger.Sobrenome = reader.GetString(2);
                            passanger.Email = reader.GetString(3);
                            passangers.Add(passanger);
                            try
                            {
                                passanger.AirplaneId = reader.GetInt32(4);
                            }
                            catch (Exception)
                            {
                                passanger.AirplaneId = -1;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }
            return passangers;
        }

        public List<Airplane> AllAirplanes()
        {
            str = "USE FligthDB;" +
                   "SELECT * FROM Airplane;";
            SqlCommand command;
            List<Airplane> airplanes = new List<Airplane>();
            Airplane airplane;

            using (SqlConnection connection = myConn)
            {
                command = new SqlCommand(str, connection);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            airplane = new Airplane();
                            airplane.Id = reader.GetInt32(0);
                            airplane.Nome = reader.GetString(1);
                            airplane.Marca = reader.GetString(2);
                            airplanes.Add(airplane);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }
            return airplanes;
        }

        public HashSet<Passanger> AllPassangers()
        {
            str = "USE FligthDB;" +
                   "SELECT * FROM Passanger;";
            SqlCommand command;
            HashSet<Passanger> passangers = new HashSet<Passanger>();
            Passanger passanger;

            using (SqlConnection connection = myConn)
            {
                command = new SqlCommand(str, connection);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            passanger = new Passanger();
                            passanger.Id = reader.GetInt32(0);
                            passanger.Nome = reader.GetString(1);
                            passanger.Sobrenome = reader.GetString(2);
                            passanger.Email = reader.GetString(3);
                            try
                            {
                                passanger.AirplaneId = reader.GetInt32(4);
                            } catch (Exception)
                            {
                                passanger.AirplaneId = -1;
                            }
                            passangers.Add(passanger);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }
            return passangers;
        }
    }
}