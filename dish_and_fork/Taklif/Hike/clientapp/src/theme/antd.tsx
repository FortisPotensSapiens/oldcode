import { ConfigProvider } from 'antd';
import React from 'react';

const calcFontSize = (expectedBodyFontSize: number) => {
  return (14 / 16) * expectedBodyFontSize;
};

const AntdThemeProvider: React.FC = ({ children }) => {
  return (
    <ConfigProvider
      theme={{
        token: {
          colorPrimary: '#7E004A',
          fontSize: calcFontSize(18),
          fontFamily: '"Roboto","Helvetica","Arial",sans-serif',
        },
      }}
    >
      {children}
    </ConfigProvider>
  );
};

export default AntdThemeProvider;
