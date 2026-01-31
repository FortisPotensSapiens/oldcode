import styled from '@emotion/styled';
import { Button } from 'antd';

import { useGetMyPartnership } from '~/api';
import { useConfig } from '~/contexts';

const StyledExternalIdIsNotExists = styled.div({
  position: 'sticky',
  top: '0px',
  alignSelf: 'flex-start',
  zIndex: 999,
  width: '100%',
  padding: '10px',
  backgroundColor: 'rgb(34,139,34, 0.7)',
  fontSize: '90%',
  color: 'white',
  borderBottom: '1px solid white',
});

const Container = styled('div')`
  display: flex;

  @media (max-width: 800px) {
    display: block;
  }
`;

const StyledButton = styled(Button)`
  font-size: 100% !important;
  margin-left: 1rem;

  @media (max-width: 800px) {
    margin-left: 0rem;
  }
`;

const SellerNotifications = () => {
  const { data, isFetched } = useGetMyPartnership();
  const config = useConfig();

  return (
    <>
      {!data?.externalId && isFetched ? (
        <StyledExternalIdIsNotExists>
          <Container>
            <div>
              Осталось совсем чуть-чуть! Для того чтобы продавать товары, пожалуйста, зарегистрируйтесь в платежной
              системе.
            </div>
            <StyledButton size="small" target="_blank" href={config.yooReferalLink}>
              Зарегистрироваться
            </StyledButton>
          </Container>
        </StyledExternalIdIsNotExists>
      ) : undefined}
    </>
  );
};

export { SellerNotifications };
