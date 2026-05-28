// Adiciona estilização para o layout do projeto (sidebar, navbar e content)
document.addEventListener("DOMContentLoaded", function () {

    // Elemnetos do Document Object Model
    const htmlEl = document.documentElement;
    const sidebarToggleBtn = document.getElementById("sidebarToggleBtn");
    const profileDropdownBtn = document.getElementById("profileDropdownBtn");
    const profileDropdownMenu = document.getElementById("profileDropdownMenu");
    const dropdownArrow = document.getElementById("dropdownArrow");

    // Elementos Mobile
    const sidebar = document.getElementById("sidebar");
    const sidebarOverlay = document.getElementById("sidebarOverlay");
    const closeSidebarMobileBtn = document.getElementById("closeSidebarMobileBtn");

    // Função que controla a sidebar
    function toggleSidebar() {
        const isMobile = window.innerWidth < 768;

        if (isMobile) {
            // Se estiver em mobile
            if (sidebar.classList.contains("-translate-x-full")) {
                openMobileSidebar();
            } else {
                closeMobileSidebar();
            }
        } else {
            // Se estiver em desktop, colapsa a sidebar e salva o estado no localStorage
            if (htmlEl.classList.contains("sidebar-is-collapsed")) {
                htmlEl.classList.remove("sidebar-is-collapsed");
                localStorage.setItem("dpsn-sidebar-state", "expanded");
            } else {
                htmlEl.classList.add("sidebar-is-collapsed");
                localStorage.setItem("dpsn-sidebar-state", "collapsed");
            }
        }
    }

    // Verifica o estado da sidebar no localStorage e aplica a classe correspondente
    function openMobileSidebar() {
        sidebar.classList.remove("-translate-x-full");
        sidebarOverlay.classList.remove("hidden");
        setTimeout(() => {
            sidebarOverlay.classList.add("opacity-100");
        }, 10);
    }

    // Função para fechar a sidebar em mobile
    function closeMobileSidebar() {
        sidebar.classList.add("-translate-x-full");
        sidebarOverlay.classList.remove("opacity-100");
        setTimeout(() => {
            sidebarOverlay.classList.add("hidden");
        }, 300);
    }

    // Ouvintes de evento da Sidebar
    if (sidebarToggleBtn) sidebarToggleBtn.addEventListener("click", toggleSidebar);
    if (closeSidebarMobileBtn) closeSidebarMobileBtn.addEventListener("click", closeMobileSidebar);
    if (sidebarOverlay) sidebarOverlay.addEventListener("click", closeMobileSidebar);

    // Reajusta classes se a tela mudar de tamanho de forma dinâmica
    window.addEventListener("resize", function () {
        if (window.innerWidth >= 768) {
            sidebarOverlay.classList.add("hidden");
            sidebarOverlay.classList.remove("opacity-100");
            if (sidebar.classList.contains("-translate-x-full")) {
                sidebar.classList.remove("-translate-x-full");
            }
        } else {
            if (!sidebar.classList.contains("-translate-x-full")) {
                sidebar.classList.add("-translate-x-full");
            }
        }
    });

    // Função que controla o dropdown do perfil
    function toggleDropdown(e) {
        e.stopPropagation();

        const isClosed = profileDropdownMenu.classList.contains("pointer-events-none");

        if (isClosed) {
            // Abrir Dropdown com transição suave
            profileDropdownMenu.classList.remove("pointer-events-none", "opacity-0", "scale-95");
            profileDropdownMenu.classList.add("opacity-100", "scale-100");
            if (dropdownArrow) dropdownArrow.classList.add("rotate-180");
        } else {
            // Fechar
            closeDropdown();
        }
    }

    function closeDropdown() {
        if (profileDropdownMenu && !profileDropdownMenu.classList.contains("pointer-events-none")) {
            profileDropdownMenu.classList.add("pointer-events-none", "opacity-0", "scale-95");
            profileDropdownMenu.classList.remove("opacity-100", "scale-100");
            if (dropdownArrow) dropdownArrow.classList.remove("rotate-180");
        }
    }

    if (profileDropdownBtn) profileDropdownBtn.addEventListener("click", toggleDropdown);

    // Fechar ao clicar em qualquer lugar fora do menu do perfil
    document.addEventListener("click", function (e) {
        if (profileDropdownMenu && !profileDropdownMenu.contains(e.target) && !profileDropdownBtn.contains(e.target)) {
            closeDropdown();
        }
    });
});