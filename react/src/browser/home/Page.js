require('rc-slider/assets/index.css');

import React, { PropTypes as RPT, PureComponent as Component } from 'react';
import Slider from 'rc-slider';
import { connect } from 'react-redux';
import { fetchReferences, deleteAllReference, putReference, startProcessing } from '../../common/api/actions';
import { FileUpload } from 'redux-file-upload';
import { LineChart } from 'react-d3-components';

@connect(state => ({
  references: state.api.getIn(['references', 'data']),
  referencesError: state.api.getIn(['references', 'error']),
  referencesPending: state.api.getIn(['references', 'pending']),
  dtw: state.api.getIn(['dtw', 'data']),
  dtwError: state.api.getIn(['dtw', 'error']),
  dtwPending: state.api.getIn(['dtw', 'pending']),
  uploadPending: state.app.get('uploadPending'),
}), { fetchReferences, deleteAllReference, putReference, startProcessing })
export default class Page extends Component {

  static propTypes = {
    deleteAllReference: RPT.func,
    dtw: RPT.array,
    dtwError: RPT.bool,
    dtwPending: RPT.bool,
    fetchReferences: RPT.func.isRequired,
    putReference: RPT.func,
    references: RPT.array,
    referencesError: RPT.bool,
    referencesPending: RPT.bool,
    uploadPending: RPT.bool,
  }

  componentDidMount() {
    const { fetchReferences } = this.props;
    fetchReferences();
  }

  renderImages(id, imgUrl, contourUrl, cannyTreshodLinking, cannyTreshold) {
    const { putReference } = this.props;
    return (
      <div style={styles.imageWrapper}>
        <img src={imgUrl} style={styles.image} />
        <img src={`${contourUrl}?${Math.random()}`} style={styles.image} />
        <p>cannyThreshold</p>
        <Slider
          max={250}
          min={5}
          step={5}
          onAfterChange={(x) => putReference(id, x, cannyTreshodLinking)}
          defaultValue={cannyTreshold}
        />
        <p>cannyThresholdLinking</p>
        <Slider
          max={250}
          min={5}
          step={5}
          onAfterChange={(x) => putReference(id, cannyTreshold, x)}
          defaultValue={cannyTreshodLinking}
        />
      </div>
    );
  }

  render() {
    const { references, referencesError, referencesPending, deleteAllReference, startProcessing, uploadPending } = this.props;
    console.log(references && references.toJS())

    return (
      <div>
        <div>
          <h1>Referenční obrázky</h1>
          {references && references.map((x, key) => this.renderImages(key, x.get('imageUrl'), x.get('contourImageUrl'), x.get('cannyTreshodLinking'), x.get('cannyTreshold')))}
          <button onClick={() => deleteAllReference()}>Smazat vše</button>
          <FileUpload
            allowedFileTypes={['jpg', 'jpeg', 'png']}
            data={{ type: 'picture' }}
            dropzoneId="fileUpload"
            multiple
            url="/webapi/reference"
          >
            <button>
              Klikněte nebo přetáhněte referenční obrázky (JPG)
            </button>
          </FileUpload>
        </div>
        {referencesError && <p>Došlo k chybě. Určitě běží API?</p>}
        {uploadPending && <p>Odesílám fotky...</p>}
        {referencesPending && <p>Stahuji data a zpracovávám grafy...</p>}

        <h1>Obrázky k porovnání</h1>

        <button onClick={() => startProcessing()}>Spustit výpočet</button>
      </div>
    );
  }
}

const styles = {
  imageWrapper: {
    clear: 'both'
  },
  image: {
    width: '200px',
    marginLeft: '5px'
  }
}
