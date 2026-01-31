import { CopyOutlined } from '@ant-design/icons';
import styled from '@emotion/styled';
import { Button, Card, Popover, Typography } from 'antd';
import { FC } from 'react';
import { CopyToClipboard } from 'react-copy-to-clipboard';

import { useConfig } from '~/contexts';

const StyledCard = styled(Card)`
  width: 730px;

  @media (max-width: 900px) {
    width: 100%;
  }
`;

const ExternalIdText = styled('strong')`
  padding: 0 0.5rem;
`;

const StyledTitle = styled(Typography.Title)`
  margin-top: 0;
`;

const PartnerExternalIdView: FC<{
  externalId?: string;
}> = ({ externalId }) => {
  const config = useConfig();

  return (
    <StyledCard>
      <StyledTitle level={4}>Платежная система</StyledTitle>
      {externalId ? (
        <>
          <span>Идентификатор платежной системы:</span>
          <ExternalIdText>{externalId}</ExternalIdText>
          <CopyToClipboard text={externalId}>
            <Popover title="Скопировано в буфер обмена!" trigger="click" placement="right">
              <Button icon={<CopyOutlined />} />
            </Popover>
          </CopyToClipboard>
        </>
      ) : (
        <>
          Подключите платежную систему.{' '}
          <Button type="primary" target="_blank" href={config.yooReferalLink}>
            Подключить
          </Button>
        </>
      )}
    </StyledCard>
  );
};

export { PartnerExternalIdView };
