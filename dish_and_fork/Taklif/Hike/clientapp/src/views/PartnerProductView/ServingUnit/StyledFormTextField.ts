import { styled } from '@mui/material';

import { FormTextField, FormTextFieldProps } from '../FormTextField';

type StyledFormTextFieldProps = FormTextFieldProps & {
  hovered?: boolean;
  noBorder?: 'left' | 'right';
  width?: number | string;
};

const StyledFormTextField = styled(FormTextField, {
  name: 'StyledFormTextField',
  shouldForwardProp: (prop) => prop !== 'noBorder' && prop !== 'width' && prop !== 'hovered',
})<StyledFormTextFieldProps>(({ hovered, noBorder, theme, width }) => {
  const widthStyles = width
    ? {
        flexBasis: width,
        flexGrow: 0,
        flexShrink: 0,
        width,
      }
    : { flexGrow: 1 };

  const common = {
    '& .MuiInputBase-root:not(.Mui-error) .MuiOutlinedInput-notchedOutline': {
      borderColor: hovered ? theme.palette.text.primary : undefined,
    },
    ...widthStyles,
  };

  switch (noBorder) {
    case 'left':
      return {
        ...common,

        '& .MuiInputBase-root .MuiOutlinedInput-notchedOutline': {
          borderBottomLeftRadius: 0,
          borderLeftWidth: 0,
          borderTopLeftRadius: 0,
        },
      };

    case 'right':
      return {
        ...common,

        '& .MuiInputBase-root .MuiOutlinedInput-notchedOutline': {
          borderBottomRightRadius: 0,
          borderRightWidth: 0,
          borderTopRightRadius: 0,
        },
      };

    default:
      return common;
  }
});

export { StyledFormTextField };
export type { StyledFormTextFieldProps };
