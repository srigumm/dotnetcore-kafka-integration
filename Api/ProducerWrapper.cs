namespace Api
{
    using Confluent.Kafka;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class ProducerWrapper
    {
        private string _topicName;
        private Producer<string,string> _producer;
        private ProducerConfig _config;
        private static readonly Random rand = new Random();

        public ProducerWrapper(ProducerConfig config,string topicName)
        {
            this._topicName = topicName;
            this._config = config;
            this._producer = new Producer<string,string>(this._config);
            this._producer.OnError += (_,e)=>{
                Console.WriteLine("Exception:"+e);
            };
        }
        public async Task writeMessage(string message){
            var dr = await this._producer.ProduceAsync(this._topicName, new Message<string, string>()
                        {
                            Key = rand.Next(5).ToString(),
                            Value = message
                        });
            Console.WriteLine($"KAFKA => Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
            return;
        }
    }
}