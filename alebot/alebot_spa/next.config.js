/** @type {import('next').NextConfig} */
const nextConfig = {
  pageExtensions: ['mdx', 'md', 'jsx', 'tsx'],
  typescript: {
    // !! WARN !!
    // Dangerously allow production builds to successfully complete even if
    // your project has type errors.
    // !! WARN !!
    ignoreBuildErrors: true,
  },
  eslint: {
    // Warning: This allows production builds to successfully complete even if
    // your project has ESLint errors.
    ignoreDuringBuilds: true,
  },
  reactStrictMode: true,
  swcMinify: true,
  images: {
    loader: 'custom',
    loaderFile: './loader.js',
  },
  async headers() {
    return [
      {
        source: '/(.*).jpg',
        headers: [
          {
            key: 'Cache-Control',
            value:
              'public, max-age=31536000, immutable',
          },
        ],
      },
      {
        source: '/(.*).ico',
        headers: [
          {
            key: 'Cache-Control',
            value:
              'public, max-age=31536000, immutable',
          },
        ],
      },
      {
        source: '/(.*).png',
        headers: [
          {
            key: 'Cache-Control',
            value:
              'public, max-age=31536000, immutable',
          },
        ],
      },
      {
        source: '/(.*).bmp',
        headers: [
          {
            key: 'Cache-Control',
            value:
              'public, max-age=31536000, immutable',
          },
        ],
      },
      {
        source: '/(.*).gif',
        headers: [
          {
            key: 'Cache-Control',
            value:
              'public, max-age=31536000, immutable',
          },
        ],
      },
      {
        source: '/(.*).jpeg',
        headers: [
          {
            key: 'Cache-Control',
            value:
              'public, max-age=31536000, immutable',
          },
        ],
      },
    ]
  },
};

module.exports = nextConfig;
