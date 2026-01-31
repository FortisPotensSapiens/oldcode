import styled from '@emotion/styled';
import { Modal } from 'antd';
import { FC, useState } from 'react';

import { useDownSm } from '~/hooks';

import { FilterListIconStyled, StorefrontHeaderContainer } from './styled';

const StyledContainer = styled.div`
  position: relative;
`;

const StyledDot = styled.div`
  position: absolute;
  right: -7px;
  top: 2px;
  width: 7px;
  height: 7px;
  background: #7e004a;
  border-radius: 100%;
`;

const StyledPreffix = styled.div`
  padding-right: 10px;
  flex: 1;
`;

const StorefrontHeader: FC<{
  isFilterSelected?: boolean;
  preffixSlot?: JSX.Element;
}> = ({ isFilterSelected, preffixSlot, children }) => {
  const [isModalOpen, setIsModalOpen] = useState(false);
  const isXs = useDownSm();

  const showModal = () => {
    setIsModalOpen(true);
  };

  const handleCancel = () => {
    setIsModalOpen(false);
  };

  return (
    <StorefrontHeaderContainer>
      {preffixSlot ? <StyledPreffix>{preffixSlot}</StyledPreffix> : undefined}

      <StyledContainer>
        <FilterListIconStyled onClick={showModal} />
        {isXs && isFilterSelected ? <StyledDot /> : undefined}
      </StyledContainer>

      <Modal
        open={isModalOpen}
        footer={null}
        onCancel={handleCancel}
        bodyStyle={{
          paddingTop: 40,
          paddingBottom: 0,
        }}
      >
        {children}
      </Modal>
    </StorefrontHeaderContainer>
  );
};

export default StorefrontHeader;
