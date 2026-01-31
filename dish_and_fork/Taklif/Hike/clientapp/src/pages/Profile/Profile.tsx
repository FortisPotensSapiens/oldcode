import { TabContext, TabList } from '@mui/lab';
import { Box, Paper, styled, Typography } from '@mui/material';
import { useSnackbar } from 'notistack';
import { FC, useEffect, useMemo } from 'react';

import { Error, LoadingSpinner, PageLayout } from '~/components';
import { useDownSm, useMyUserProfile } from '~/hooks';
import { UserProfileUpdateModel } from '~/types';
import { StyledTab } from '~/views/PartnerSettingsView/StyledTab';
import { StyledTabPanel } from '~/views/PartnerSettingsView/StyledTabPanel';
import { useTab } from '~/views/PartnerSettingsView/useTab';

import { AddMyExternalLogin } from './AddMyExternalLogin';
import AddPasswordForm from './AddPasswordForm';
import ChangeProfileEmailForm from './ChangeProfileEmailForm';
import { DeleteMyExternalLogin } from './DeleteMyExternalLogin';
import ProfileEditForm from './ProfileEditForm';
import UpdatePasswordForm from './UpdatePasswordForm';

const TAB_MAIN = 'main';
const TAB_EMAIL = 'email';
const TAB_PASSWORD = 'password';
const TAB_LOGINS = 'logins';

const StyledSubTitle = styled(Box)({
  fontSize: '70%',
  opacity: 0.5,
});

const StyledTitle = styled(Box)({
  overflow: 'hidden',
  textOverflow: 'ellipsis',
  width: '100%',
  whiteSpace: 'nowrap',
});

const Profile: FC = () => {
  const { enqueueSnackbar } = useSnackbar();
  const { data, isError, isLoading, refetch } = useMyUserProfile({ refetchOnMount: false });
  const defaultValues = useMemo<Pick<UserProfileUpdateModel, 'userName'>>(
    () => ({ userName: data?.userName ?? '' }),
    [data],
  );

  useEffect(() => {
    if (isError || !data) {
      enqueueSnackbar('Ошибка получения профиля', {
        variant: 'error',
      });
    }
  }, [isError, enqueueSnackbar, data]);

  const isSmall = useDownSm();
  const tabOrientation = useMemo(() => (isSmall ? 'vertical' : 'horizontal'), [isSmall]);
  const [tab, changeTab] = useTab(TAB_MAIN);

  if (isError || !data) {
    return <Error />;
  }

  if (isLoading) {
    return <LoadingSpinner />;
  }

  return (
    <PageLayout
      title={
        <>
          <StyledTitle>{data.userName}</StyledTitle>
          <StyledSubTitle>Пользователь</StyledSubTitle>
        </>
      }
    >
      <TabContext value={tab}>
        <TabList onChange={changeTab} orientation={tabOrientation}>
          <StyledTab label="Свободные данные профиля" value={TAB_MAIN} />
          <StyledTab label="Email" value={TAB_EMAIL} />
          <StyledTab label="Пароль" value={TAB_PASSWORD} />
          <StyledTab label="Внешние логины" value={TAB_LOGINS} />
        </TabList>

        <StyledTabPanel value={TAB_MAIN}>
          <ProfileEditForm data={defaultValues} onUpdated={refetch} />
        </StyledTabPanel>

        <StyledTabPanel value={TAB_EMAIL}>
          <ChangeProfileEmailForm data={{ newEmail: data?.email ?? '' }} onUpdated={refetch} />
        </StyledTabPanel>

        <StyledTabPanel value={TAB_PASSWORD}>
          {data?.hasPassword ? <UpdatePasswordForm onUpdated={refetch} /> : <AddPasswordForm onUpdated={refetch} />}
        </StyledTabPanel>

        <StyledTabPanel value={TAB_LOGINS}>
          <Box borderRadius={{ sm: 2.5, xs: 0 }} component={Paper} p={3}>
            {!!data.logins?.length && <Typography variant="h5">Добавленные внешние способы входа</Typography>}

            {data.logins?.map(({ loginProvider, providerDisplayName, providerKey }, index) =>
              loginProvider && providerKey ? (
                <DeleteMyExternalLogin
                  key={`${loginProvider}_${index.toString()}`}
                  disabled={!data.showRemoveLoginButton}
                  loginProvider={loginProvider}
                  onUpdated={refetch}
                  providerDisplayName={providerDisplayName}
                  providerKey={providerKey}
                />
              ) : null,
            )}

            {!!data.otherLogins?.length && <Typography variant="h5">Доступные новые внешние способы входа</Typography>}

            {data.otherLogins?.map(
              ({ displayName, name }, index) =>
                name && (
                  <AddMyExternalLogin
                    key={`${name}_${index.toString()}`}
                    displayName={displayName}
                    loginProvider={name}
                    onUpdated={refetch}
                  />
                ),
            )}
          </Box>
        </StyledTabPanel>
      </TabContext>
    </PageLayout>
  );
};

export { Profile };
