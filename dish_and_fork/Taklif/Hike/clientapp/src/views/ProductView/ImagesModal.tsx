import { LeftOutlined, RightOutlined } from '@ant-design/icons';
import styled from '@emotion/styled';
import { Box } from '@mui/material';
import { Modal } from 'antd';
import Carousel from 'nuka-carousel';
import { FC } from 'react';

import { FileReadModel } from '~/types';

const IconBox = styled(Box)`
  padding: 10px;
  background: rgba(255, 255, 255, 0.5);
`;

const StyledContainer = styled('div')`
  height: 100%;

  .slider-container {
    height: 100%;
  }

  .slider-frame {
    height: 100% !important;
  }

  .slider-list {
    height: 100% !important;
  }

  .slide {
    text-align: center;
  }

  img {
    height: 100%;
  }

  @media (orientation: portrait) {
    img {
      height: auto;
      width: 100%;
    }
  }
`;

const StyledModal = styled(Modal)`
  .ant-modal-close-x {
    background: rgba(255, 255, 255, 0.8);
    padding: 10px;
  }

  @media (orientation: portrait) {
    .ant-modal-content {
      height: 400px;
    }

    .ant-modal-body {
      height: 380px !important;
    }
  }
`;

const ImagesModal: FC<{
  open: boolean;
  images: FileReadModel[];
  current: number;
  onCancel: () => void;
}> = ({ open, images, current, onCancel }) => {
  return (
    <StyledModal
      open={open}
      onCancel={onCancel}
      width="90%"
      style={{
        top: '10%',
        height: '80vh',
      }}
      footer={false}
      bodyStyle={{
        height: '80vh',
      }}
    >
      {images?.length ? (
        <StyledContainer>
          <Carousel
            slideIndex={current ?? 0}
            cellAlign="center"
            renderCenterLeftControls={(props) => {
              return !props.previousDisabled ? (
                <IconBox onClick={() => props.previousSlide()}>
                  <LeftOutlined />
                </IconBox>
              ) : (
                <></>
              );
            }}
            renderCenterRightControls={(props) => {
              return !props.nextDisabled ? (
                <IconBox onClick={() => props.nextSlide()}>
                  <RightOutlined />
                </IconBox>
              ) : (
                <></>
              );
            }}
          >
            {images?.map((image) => {
              return (
                // eslint-disable-next-line jsx-a11y/click-events-have-key-events, jsx-a11y/no-noninteractive-element-interactions
                <img alt="" key={image.id} src={String(image.path)} />
              );
            })}
          </Carousel>
        </StyledContainer>
      ) : undefined}
    </StyledModal>
  );
};

export { ImagesModal };
