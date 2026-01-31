import { Box, Button, ButtonProps } from '@mui/material';
import { FC } from 'react';
import { Link } from 'react-router-dom';

import { useClick } from './useClick';

type EmptyListProps = Pick<ButtonProps, 'onClick'> & {
  buttonText?: string;
  to?: string;
};

const EmptyList: FC<EmptyListProps> = ({ buttonText = 'Выбрать изделие', children, onClick, to }) => {
  const click = useClick(onClick);

  return (
    <Box alignItems="center" color="common.black" display="flex" flexGrow="1" justifyContent="center" pb={8}>
      <Box alignItems="center" display="flex" flexDirection="column">
        <Box mb={2.5}>{children}</Box>

        {to ? (
          <Button component={Link} to={to} variant="contained">
            {buttonText}
          </Button>
        ) : (
          <Button onClick={click} variant="contained">
            {buttonText}
          </Button>
        )}
      </Box>
    </Box>
  );
};

export { EmptyList };
export type { EmptyListProps };
