const CracoAlias = require('craco-alias');

module.exports = {
  plugins: [
    {
      options: {
        baseUrl: './src',
        source: 'tsconfig',
        tsConfigPath: './tsconfig.paths.json',
      },
      plugin: CracoAlias,
    },
  ],
};
