import 'dayjs/locale/ru';
import '../i18n';
import './App.less';

import dayjs from 'dayjs';
import { FC } from 'react';

import { AgreementWrapper } from '~/components/AgreementWrapper/AgreementWrapper';

import { AppProviders } from './AppProviders';
import { Prepare } from './Prepare';
import { FirebaseProvider } from './providers/FirebaseProvider';
import { Routes } from './Routes';

dayjs.locale('ru');

const App: FC = () => {
  return (
    <AppProviders>
      <Prepare>
        <AgreementWrapper>
          <FirebaseProvider>
            <Routes />
          </FirebaseProvider>
        </AgreementWrapper>
      </Prepare>
    </AppProviders>
  );
};

export { App };
