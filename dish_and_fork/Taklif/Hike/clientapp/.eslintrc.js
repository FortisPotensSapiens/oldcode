module.exports = {
  env: {
    browser: true,
  },

  extends: [
    'eslint:recommended',
    'airbnb',
    'plugin:@typescript-eslint/recommended',
    'plugin:import/typescript',
    'plugin:react/recommended',
    'react-app',
    'plugin:prettier/recommended',
  ],

  overrides: [
    {
      files: ['*.js'],
      rules: {
        '@typescript-eslint/explicit-function-return-type': 'off',
        '@typescript-eslint/no-var-requires': 'off',
      },
    },

    {
      files: ['*.ts', '*.tsx'],
      rules: {
        'import/no-unresolved': 'off',
        'no-nested-ternary': 'off',
        'no-use-before-define': 'off',
      },
    },
  ],

  plugins: ['prettier', 'sort-keys-fix', 'simple-import-sort', 'sort-destructure-keys', 'react', 'react-hooks'],

  rules: {
    '@typescript-eslint/indent': 'off',
    '@typescript-eslint/no-object-literal-type-assertion': 'off',
    '@typescript-eslint/no-use-before-define': 'off',

    'import/extensions': 'off',
    'import/no-extraneous-dependencies': ['error', { bundledDependencies: true }],
    'import/no-named-as-default': 'off',
    'import/no-unresolved': ['error', { ignore: ['.svg$', '^@/'] }],
    'import/prefer-default-export': 'off',

    'jsx-a11y/label-has-associated-control': [
      'error',
      {
        controlComponents: [],
        depth: 3,
        labelAttributes: [],
        labelComponents: [],
      },
    ],
    'jsx-a11y/label-has-for': ['error', { allowChildren: true }],
    'jsx-quotes': ['error', 'prefer-double'],

    'max-len': [
      'error',
      {
        code: 120,
        ignoreComments: true,
        ignoreRegExpLiterals: true,
        ignoreStrings: true,
        ignoreTemplateLiterals: true,
        ignoreTrailingComments: true,
        ignoreUrls: true,
      },
    ],

    'no-console':
      process.env.NODE_ENV === 'production'
        ? ['error', { allow: ['error', 'info', 'warn'] }]
        : ['warn', { allow: ['error', 'info', 'warn'] }],
    'no-debugger': process.env.NODE_ENV === 'production' ? 'error' : 'warn',
    'no-param-reassign': 'off',
    'no-restricted-exports': 'off',
    'no-shadow': 'off',
    'no-use-before-define': ['error', { functions: false }],

    'object-curly-spacing': [
      'error',
      'always',
      {
        arraysInObjects: true,
      },
    ],

    'react/function-component-definition': 'off',
    'react/jsx-filename-extension': ['error', { extensions: ['.js', '.jsx', '.ts', '.tsx'] }],
    'react/jsx-no-useless-fragment': 'off',
    'react/jsx-props-no-spreading': 'off',
    'react/jsx-sort-props': 'off',
    'react/prop-types': 'off',
    'react/react-in-jsx-scope': 'off',
    'react/require-default-props': 'off',

    'require-jsdoc': 'off',

    'simple-import-sort/exports': 'warn',

    'simple-import-sort/imports': [
      'warn',
      {
        groups: [
          ['^\\u0000'],
          ['^(@(?!(api|components|config|contexts|hooks|pages|theme|utils)))?\\w'],
          ['^'],
          ['^@dnf?\\w'],
          ['^\\.'],
          ['\\.css$', '\\.scss$'],
        ],
      },
    ],

    'sort-destructure-keys/sort-destructure-keys': 'off',
    'sort-keys-fix/sort-keys-fix': 'off',
  },
};
