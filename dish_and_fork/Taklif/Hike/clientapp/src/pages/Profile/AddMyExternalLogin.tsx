import AddIcon from '@mui/icons-material/Add';
import { Box } from '@mui/material';
import { useSnackbar } from 'notistack';
import { FC, useEffect } from 'react';

import { useAddMyExternalLogin } from '~/api';
import { AddLoginModel, AuthenticationSchemeReadModel } from '~/types';

import { StyledIconButton } from './StyledIconButton';

type AddMyExternalLoginProps = Pick<AuthenticationSchemeReadModel, 'displayName'> &
  AddLoginModel & { onUpdated: () => void };

const AddMyExternalLogin: FC<AddMyExternalLoginProps> = ({ displayName, loginProvider, onUpdated }) => {
  const { isError, isLoading, isSuccess, mutate } = useAddMyExternalLogin();
  const { enqueueSnackbar } = useSnackbar();

  const clickHandler = () => {
    mutate({ loginProvider });
  };

  useEffect(() => {
    if (isError) {
      enqueueSnackbar('Ошибка', {
        variant: 'error',
      });
      onUpdated();
    }
  }, [isError, enqueueSnackbar, onUpdated]);

  useEffect(() => {
    if (isSuccess) {
      enqueueSnackbar('Успех', {
        variant: 'success',
      });
      onUpdated();
    }
  }, [isSuccess, enqueueSnackbar, onUpdated]);

  return (
    <Box alignItems="center" display="flex" mt={2}>
      <StyledIconButton loading={isLoading} onClick={clickHandler} variant="outlined">
        <AddIcon />
      </StyledIconButton>

      {displayName ?? loginProvider}
    </Box>
  );
};

export { AddMyExternalLogin };
export type { AddMyExternalLoginProps };
