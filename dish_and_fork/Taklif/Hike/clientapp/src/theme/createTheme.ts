import { createTheme as createMuiTheme, Theme } from '@mui/material';

declare module '@mui/material/styles/createPalette' {
  interface CommonColors {
    grey: string;
  }

  interface TypeText {
    blue: string;
    lightGrey: string;
  }
}

declare module '@mui/system/createTheme/shape' {
  interface Shape {
    bigBorderRadius: number | string;
  }
}

type CreateTheme = () => Theme;

const spacing = 8;
const oneAndHalfQuarter = 8 * 1.25;
const oneAndHalfSpacing = 8 * 1.5;

const createTheme: CreateTheme = () => {
  const theme = createMuiTheme({
    components: {
      MuiAlert: {
        styleOverrides: {
          message: {
            lineHeight: 1.5,
            padding: 0,
          },

          root: {
            borderRadius: oneAndHalfQuarter,
            fontSize: '1rem',
            padding: spacing,
            paddingLeft: oneAndHalfSpacing,
            paddingRight: oneAndHalfSpacing,
          },
        },
        variants: [
          {
            props: {
              severity: 'warning',
            },
            style: {
              color: '#FFB800',
            },
          },
        ],
      },
    },

    palette: {
      background: {
        default: '#FAFAFA',
        paper: '#FFFFFF',
      },

      common: {
        grey: '#C4C4C4',
      },

      info: {
        contrastText: '#000000',
        dark: '#DEDEDE',
        light: '#F6F6F6',
        main: '#F6F6F6',
      },

      primary: {
        main: '#7E004A',
      },

      success: {
        contrastText: '#FFFFFF',
        dark: '#3B873E',
        light: '#7BC67E',
        main: '#4CAF50',
      },

      text: {
        blue: '#4240A8',
        lightGrey: '#ACABAB',
      },
    },

    shadows: [
      'none',
      '-2px -2px 2px rgba(0, 0, 0, 0.02), 2px 2px 2px rgba(0, 0, 0, 0.02);',
      '0px 3px 1px -2px rgba(0, 0, 0, 0.2), 0px 2px 2px rgba(0, 0, 0, 0.14), 0px 1px 5px rgba(0, 0, 0, 0.12)',
      '-2px -2px 2px rgba(0, 0, 0, 0.02), 2px 2px 2px rgba(0, 0, 0, 0.02)',
      '-2px -2px 2px rgba(0, 0, 0, 0.02), 2px 2px 2px rgba(0, 0, 0, 0.02)',
      '-2px -2px 2px rgba(0, 0, 0, 0.02), 2px 2px 2px rgba(0, 0, 0, 0.02)',
      '-2px -2px 2px rgba(0, 0, 0, 0.02), 2px 2px 2px rgba(0, 0, 0, 0.02)',
      '-2px -2px 2px rgba(0, 0, 0, 0.02), 2px 2px 2px rgba(0, 0, 0, 0.02)',
      '-2px -2px 2px rgba(0, 0, 0, 0.02), 2px 2px 2px rgba(0, 0, 0, 0.02)',
      '-2px -2px 2px rgba(0, 0, 0, 0.02), 2px 2px 2px rgba(0, 0, 0, 0.02)',
      '-2px -2px 2px rgba(0, 0, 0, 0.02), 2px 2px 2px rgba(0, 0, 0, 0.02)',
      '-2px -2px 2px rgba(0, 0, 0, 0.02), 2px 2px 2px rgba(0, 0, 0, 0.02)',
      '-2px -2px 2px rgba(0, 0, 0, 0.02), 2px 2px 2px rgba(0, 0, 0, 0.02)',
      '-2px -2px 2px rgba(0, 0, 0, 0.02), 2px 2px 2px rgba(0, 0, 0, 0.02)',
      '-2px -2px 2px rgba(0, 0, 0, 0.02), 2px 2px 2px rgba(0, 0, 0, 0.02)',
      '-2px -2px 2px rgba(0, 0, 0, 0.02), 2px 2px 2px rgba(0, 0, 0, 0.02)',
      '-2px -2px 2px rgba(0, 0, 0, 0.02), 2px 2px 2px rgba(0, 0, 0, 0.02)',
      '-2px -2px 2px rgba(0, 0, 0, 0.02), 2px 2px 2px rgba(0, 0, 0, 0.02)',
      '-2px -2px 2px rgba(0, 0, 0, 0.02), 2px 2px 2px rgba(0, 0, 0, 0.02)',
      '-2px -2px 2px rgba(0, 0, 0, 0.02), 2px 2px 2px rgba(0, 0, 0, 0.02)',
      '-2px -2px 2px rgba(0, 0, 0, 0.02), 2px 2px 2px rgba(0, 0, 0, 0.02)',
      '-2px -2px 2px rgba(0, 0, 0, 0.02), 2px 2px 2px rgba(0, 0, 0, 0.02)',
      '-2px -2px 2px rgba(0, 0, 0, 0.02), 2px 2px 2px rgba(0, 0, 0, 0.02)',
      '-2px -2px 2px rgba(0, 0, 0, 0.02), 2px 2px 2px rgba(0, 0, 0, 0.02)',
      '-2px -2px 2px rgba(0, 0, 0, 0.02), 2px 2px 2px rgba(0, 0, 0, 0.02)',
    ],

    shape: {
      bigBorderRadius: 2.5,
    },

    spacing,
  });

  theme.breakpoints.values.xl = 1440;

  return theme;
};

export default createTheme;
