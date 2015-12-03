using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoNest
{
    public class Mock
    {
        public string GetArtistInfo()
        {
            
            var sb = new StringBuilder();
                
            sb.AppendLine("Lorem ipsum dolor sit amet, consectetur adipiscing elit.Cras commodo consequat consectetur. Pellentesque nec tortor viverra, tristique tortor ac, pharetra lacus.Integer nec magna eu diam congue aliquam id eget lectus. In quam erat, vulputate quis iaculis sed, imperdiet in nisl.Sed suscipit convallis enim at euismod. Vivamus non nulla finibus, pulvinar lorem malesuada, feugiat est.Aenean quis magna consequat, commodo orci eu, vestibulum tellus.Ut sit amet lorem non turpis luctus hendrerit nec egestas nunc.");
            sb.AppendLine("Aenean pharetra, nisl vel feugiat tristique, libero magna cursus orci, suscipit aliquam massa dui et massa.Aliquam eu rutrum nulla. Ut turpis dolor, scelerisque et est viverra, scelerisque tincidunt elit. Vivamus convallis rhoncus congue. In lobortis lectus nibh, in suscipit nulla rhoncus nec. Duis pellentesque, felis id sodales luctus, libero elit maximus diam, et sollicitudin sapien nisi nec ex.Vivamus imperdiet erat eu purus molestie, vel accumsan eros semper.Nulla non nisl vestibulum, fringilla mauris commodo, sagittis eros.");
            sb.AppendLine("Fusce feugiat blandit augue, sed elementum enim laoreet eget. Sed non est eu tortor accumsan porttitor non sollicitudin dolor. Praesent vitae odio ligula. Nulla suscipit augue justo, eu molestie erat interdum nec. Duis feugiat tempus massa vitae eleifend. Integer sit amet mauris justo.Nulla mollis quis mauris sed pretium.");
            sb.AppendLine("Donec pharetra faucibus nisl, in mollis ipsum blandit quis. Cras vulputate dui quis tellus pharetra placerat.Aliquam varius faucibus elit sit amet convallis.Fusce massa nunc, bibendum eget ultricies quis, congue sed enim. Pellentesque neque enim, consequat vitae diam sit amet, laoreet tempus lacus.Pellentesque quis tellus eu ligula sodales lobortis sit amet nec ante.Duis sodales vel ante id commodo. Nam suscipit posuere mauris, non efficitur nisl vehicula eu. Nulla accumsan rutrum ligula. Morbi tempus eros et dolor malesuada facilisis.Nunc et condimentum dui. Nulla in nisi quis velit pulvinar aliquet non at libero. Pellentesque varius mi ut magna venenatis, sit amet sodales erat scelerisque. Pellentesque euismod finibus tincidunt. Morbi tincidunt, orci ut laoreet vulputate, magna libero vulputate felis, ut ultrices est orci eu libero.");
            sb.AppendLine("Vestibulum auctor eleifend pulvinar. Pellentesque ut dolor eu urna lobortis lobortis.Aenean quis orci ex. In eget mattis turpis. Donec non lorem fringilla, volutpat nunc a, imperdiet metus.Mauris dignissim libero sed est scelerisque, sed commodo enim semper.Mauris lacinia sapien non leo sollicitudin ultricies.Integer et tellus nec nunc commodo tristique ullamcorper sed purus. Vestibulum mollis sollicitudin eros quis ultrices.");

            return sb.ToString();
           
        }
    }
}
