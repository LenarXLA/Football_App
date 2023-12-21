using System;
using Football_App.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Football_App.Utilities;

namespace Football_App.Service
{
    public class DBRepository
    {

        #region Clubs

        public List<Club> GetClubs()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.Clubs
                    .Include(c => c.FootballPlayers)
                    .ToList();
            }

        }
        
        public Club GetClubById(int id)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.Clubs
                    .Include(c => c.FootballPlayers)
                    .FirstOrDefault(p => p.ClubId == id);
            }
        }
        
        public void AddClub(Club club)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Clubs.Add(club);
                context.SaveChanges();
            }
        }

        public void RemoveClub(Club club)
        {

            // удаляем связи - привязанных футболистов
            var fp = GetPlayersByClubId(club);
            if (fp.Count > 0)
            {
                using (DatabaseContext context = new DatabaseContext())
                {
                    foreach (var el in fp)
                    {
                        el.ClubId = null;
                        context.FootballPlayers.Update(el);
                    }
                    context.SaveChanges();
                }
            }
            
            // удаляем клуб в новом контексте для исключения ошибок
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Clubs.Remove(club);
                context.SaveChanges();
            }
        }

        public void EliminatePlayerFromClub(FootballPlayer player)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                player.ClubId = null;
                player.Club = null;

                context.FootballPlayers.Update(player);
                context.SaveChanges();
            }
        }

        #endregion

        #region Players

        public List<FootballPlayer> GetPlayers()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.FootballPlayers
                    .Include(f => f.Club)
                    .ToList();
            }
        }

        public List<FootballPlayer> GetFreePlayers()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.FootballPlayers.Where(p => p.ClubId == null).ToList();
            }
        }

        public List<FootballPlayer> GetPlayersByClubId(Club club)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.FootballPlayers.Where(f => f.ClubId == club.ClubId).ToList();
            }
        }
        
        public void RemovePlayer(FootballPlayer player)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.FootballPlayers.Remove(player);
                context.SaveChanges();
            }
        }

        public void AddPlayerToClub(FootballPlayer player, Club club)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                player.ClubId = club.ClubId;

                context.FootballPlayers.Update(player);
                context.SaveChanges();
            }
        }
        
        public void AddPlayer(FootballPlayer player)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.FootballPlayers.Add(player);
                context.SaveChanges();
            }
        }

        public void ChangePlayerClub(FootballPlayer player, Club club)
        {
            EliminatePlayerFromClub(player);
            AddPlayerToClub(player, club);
        }
        
        #endregion
        
        #region Fans

        public List<Club> GetClubsByFan(Fan fan)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.Clubs
                    .Include(c => c.FanClubs)
                    .Where(cl => cl.FanClubs.Any(f => f.FanId == fan.FanId))
                    .ToList();
            }
        }
        
        public List<Fan> GetFans()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.Fans
                    .Include(f => f.FanClubs)
                    .ToList();
            }
        }
        
        public void AddFan(Fan fan)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Fans.Add(fan);
                context.SaveChanges();
            }
        }

        public List<Club> GetAvailableClubs(Fan fan)
        {
            List<Club> allClubs = GetClubs();
            List<Club> trackedClubs = GetClubsByFan(fan);
            
            // Вычитаем разность множеств, с помощью сравнения ClubId с кастомным ClubIdComparer
            return allClubs.Except(trackedClubs, new ClubIdComparer()).ToList();
        }
        
        public void AddAvailableClubToFan(Fan fan, Club club)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                FanClub fc = new FanClub()
                {
                    ClubId = club.ClubId,
                    FanId = fan.FanId
                };

                context.FanClubs.Add(fc);
                
                context.SaveChanges();
            }
        }
        
        public void UnlinkClubFromFan(Fan fan, Club club)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                FanClub fc = context.FanClubs
                    .FirstOrDefault(fc => fc.FanId == fan.FanId && fc.ClubId == club.ClubId);
                if (fc != null)
                {
                    context.FanClubs.Remove(fc);
                    context.SaveChanges();
                }
            }
        }
            
        public void RemoveFan(Fan fan)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                // удаляем связи - привязанные клубы
                var fanClubs = GetClubsByFan(fan);
                if (fanClubs.Count > 0)
                {
                    foreach (var cl in fanClubs)
                    {
                        FanClub fc = context.FanClubs
                            .FirstOrDefault(fc => fc.FanId == fan.FanId && fc.ClubId == cl.ClubId);
                        if (fc != null)
                        {
                            context.FanClubs.Remove(fc);
                        }
                    }
                }
                
                context.Fans.Remove(fan);
                context.SaveChanges();
            }
        }

        #endregion

    }
}
