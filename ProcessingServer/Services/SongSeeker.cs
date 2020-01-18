using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProcessingServer.Services
{
    public class SongSeeker
    {
        Random random;
        public SongSeeker()
        {
            random = new Random();
        }
        private List<T> Pick<T>(List<T> elements, int numberOfChoices=1)
        {
            int attempts = 100;
            if (elements.Count <= numberOfChoices)
                return elements;

            int index = random.Next(elements.Count);
            var pickedElements = new List<T>();
            var pickedIndexes = new List<int>();
            pickedIndexes.Add(index);
            pickedElements.Add(elements[index]);

            while(pickedIndexes.Count<numberOfChoices && attempts>0)
            {
                index = random.Next(elements.Count);
                if(pickedIndexes.Contains(index))
                {
                    attempts--;
                }
                else
                {
                    pickedIndexes.Add(index);
                    pickedElements.Add(elements[index]);
                }
            }

            return pickedElements;
        }








    }
}
