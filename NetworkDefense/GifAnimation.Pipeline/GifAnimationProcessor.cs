namespace GifAnimation.Pipeline
{
    using Microsoft.Xna.Framework.Content.Pipeline;

    /// <summary>
    /// Part of the Gif Animation Library created by Mahdi Khodadadi Fard
    /// Use granted under the MIT License (MIT)
    /// </summary>
    [ContentProcessor(DisplayName="Gif Animation Processor")]
    internal class GifAnimationProcessor : ContentProcessor<GifAnimationContent, GifAnimationContent>
    {
        public override GifAnimationContent Process(GifAnimationContent input, ContentProcessorContext context)
        {
            return input;
        }
    }
}

