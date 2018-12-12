//
// Created by Needle on 2018-11-01.
// Copyright (c) 2018 Needle. No rights reserved :)
//

using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Red.Example.UI.Windows.Popups {

    public class CCommonPopup : WindowContract<CCommonPopup, string, CommonPopupResult> {}

    public class CommonPopup : MonoBehaviour, UIPresenter {
        public Type ContractType => typeof(CCommonPopup);

        [SerializeField] private Button okButton;
        [SerializeField] private Button cancelButton;
        [SerializeField] private Text description;

        private CCommonPopup contract;
        private CUICanvas canvas;

        private void Awake() {
            Bind();
        }

        /// <summary>
        /// Main Model-to-View binding logic
        /// </summary>
        private void Bind() {
            this.contract = this.GetOrCreate<CCommonPopup>();
            this.canvas = this.GetOrCreate<CUICanvas>();
            this.contract.OpenCommand.Subscribe(Setup);

            var selector = canvas.State.Select(s => s == CanvasStage.Opened);
            var okCommand = selector.ToReactiveCommand();

            okCommand.Subscribe(_ => Close(CommonPopupResult.Ok));
            okCommand.BindTo(this.okButton);

            var cancelCommand = selector.ToReactiveCommand();
            cancelCommand.Subscribe(_ => Close(CommonPopupResult.Cancel));
            cancelCommand.BindTo(this.cancelButton);
        }

        private void Setup(string text) {
            this.description.text = text;
        }

        /// <summary>
        /// For closing through internal code
        /// </summary>
        /// <param name="result">Result to push in underlying contract Result property</param>
        private void Close(CommonPopupResult result) {
            this.contract.Close(result);
        }
        
        /// <summary>
        /// For closing through unity UI
        /// </summary>
        /// <param name="result">Result to push in underlying contract Result property</param>
        public void Close(int result) {
            this.contract.Close((CommonPopupResult)result);
        }
    }
}